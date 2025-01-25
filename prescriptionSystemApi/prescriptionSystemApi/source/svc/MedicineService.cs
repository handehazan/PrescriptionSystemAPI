using ExcelDataReader;
using HtmlAgilityPack;
using prescriptionSystemApi.model;
using prescriptionSystemApi.source.db;
using System.Data;
using System.Globalization;
using System.Text;

namespace prescriptionSystemApi.source.svc
{
    public class MedicineService : IMedicineService
    {
        private readonly MedicineAccess _medicineAccess;
        private readonly HttpClient _httpClient;
        public MedicineService(MedicineAccess medicineAccess, HttpClient httpClient)
        {
            _medicineAccess = medicineAccess;
            _httpClient = httpClient;
        }

        public async Task<List<Medicine>> GetAllMedicinesAsync()
        {
            return await _medicineAccess.GetAllMedicinesAsync();
        }

        //Dowload the latest excel file with using web scraping
        public async Task<string> DownoadMedicineExcel()
        {
            string url = "https://www.titck.gov.tr/dinamikmodul/43";
            var response = await _httpClient.GetStringAsync(url);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            // Locate the table
            var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@id='myTable']");
            if (table == null)
            {
                Console.WriteLine("Table not found in the HTML.");
                return "Table not found in the HTML.";
            }

            // Locate the first row in the tbody
            var tbody = table.SelectSingleNode(".//tbody");
            if (tbody == null)
            {
                Console.WriteLine("Tbody not found in the table.");
                return "Tbody not found in the table.";
            }

            var firstRow = tbody.SelectSingleNode(".//tr");
            if (firstRow == null)
            {
                Console.WriteLine("First row not found in the tbody.");
                return "First row not found in the tbody.";
            }

            // Extract the file link from the first row
            var fileLinkNode = firstRow.SelectSingleNode(".//a[@class='badge']");
            if (fileLinkNode == null)
            {
                Console.WriteLine("File link not found in the first row.");
                return "File link not found in the first row.";
            }

            string fileUrl = fileLinkNode.GetAttributeValue("href", "");
            if (string.IsNullOrEmpty(fileUrl))
            {
                Console.WriteLine("File URL is empty.");
                return "File URL is empty.";
            }

            Console.WriteLine($"File URL: {fileUrl}");

            // Download the file
            var fileResponse = await _httpClient.GetAsync(fileUrl);
            if (fileResponse.IsSuccessStatusCode)
            {
                var fileBytes = await fileResponse.Content.ReadAsByteArrayAsync();
                string fileName = Path.GetFileName(fileUrl); // Extract the file name from the URL
                string fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                await System.IO.File.WriteAllBytesAsync(fullFilePath, fileBytes);
                Console.WriteLine($"File downloaded successfully: {fullFilePath}");
                return fullFilePath; // Return the full file path
            }
            else
            {
                Console.WriteLine("Failed to download the file.");
                return "Failed to download the file.";
            }
        }

        public List<Medicine> ParseExcelFile(string filePath)
        {
            var medicines = new List<Medicine>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Excel file not found.");
                return medicines;
            }

            try
            {
                Console.WriteLine($"Starting to parse Excel file: {filePath}");

                // Open the Excel file
                using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);
                

                // Read the Excel file into a DataSet
                var result = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = false, // Do not use the first row as headers
                        FilterRow = rowReader => rowReader.Depth >= 3 // Skip the first 3 rows (title, subtitle, and header)
                    }
                });

                // Get the first table (sheet) in the Excel file
                var table = result.Tables[0];

                // Log the number of rows and columns
                Console.WriteLine($"Found {table.Rows.Count} rows and {table.Columns.Count} columns.");

                // Manually map columns based on the Excel structure
                var columnMappings = new Dictionary<int, string>
                {
                    [0] = "IlacAdi",        // İlaç Adı
                    [1] = "Barkod",         // Barkod
                    [2] = "ATCCode",        // ATC Kodu
                    [3] = "ATCAdi",         // ATC Adı
                    [4] = "FirmaAdi",       // Firma Adı
                    [5] = "ReceteTuru",     // Reçete Türü
                    [6] = "Durumu",         // Durumu
                    [7] = "Aciklama",       // Açıklama
                    [8] = "TemelIlacListesiDurumu",       // Temel İlaç Listesi Durumu
                    [9] = "CocukTemelIlacListesiDurumu",  // Çocuk Temel İlaç Listesi Durumu
                    [10] = "YenidoganTemelIlacListesiDurumu", // Yenidoğan Temel İlaç Listesi Durumu
                    [11] = "AktifUrunlerListesineAlindigiTarih" // Aktif Ürünler Listesine Alındığı Tarih
                };

                // Iterate through the rows and map to the Medicine model
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        var medicine = new Medicine();

                        foreach (var mapping in columnMappings)
                        {
                            var columnIndex = mapping.Key;
                            var propertyName = mapping.Value;

                            if (columnIndex < row.ItemArray.Length)
                            {
                                var value = row[columnIndex]?.ToString();
                                var property = typeof(Medicine).GetProperty(propertyName);

                                if (property != null)
                                {
                                    if (property.PropertyType == typeof(DateTime?))
                                    {
                                        if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                                        {
                                            property.SetValue(medicine, date);
                                        }
                                    }
                                    else
                                    {
                                        property.SetValue(medicine, value);
                                    }
                                }
                            }
                        }

                        medicines.Add(medicine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing row: {ex.Message}");
                    }
                }

                Console.WriteLine($"Successfully parsed {medicines.Count} medicines.");
                return medicines;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while parsing the Excel file: {ex.Message}");
                return medicines;
            }
            /*finally
            {
                // Clean up the temporary file
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Console.WriteLine($"Temporary file {filePath} deleted.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting temporary file: {ex.Message}");
                }
            }*/
        }

        private async Task UploadMedicinesToMongoDBAsync(List<Medicine> medicines)
        {
            if (medicines == null || medicines.Count == 0)
            {
                Console.WriteLine("No medicines to upload.");
                return;
            }

            try
            {
                // Clear existing data
                await _medicineAccess.DeleteAllMedicinesAsync();

                // Insert new data
                await _medicineAccess.InsertMedicinesAsync(medicines);

                Console.WriteLine($"Uploaded {medicines.Count} medicines to MongoDB.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload medicines to MongoDB: {ex.Message}");
            }
        }

        public async Task RefreshMedicineDataAsync()
        {
            try
            {
                // Step 1: Download the Excel file
                string filePath = await DownoadMedicineExcel();

                // Check if the file was downloaded successfully
                if (System.IO.File.Exists(filePath))
                {
                     //Step 2: Parse the Excel file
                    var medicines = ParseExcelFile(filePath);

                    //Step 3: Upload medicines to MongoDB
                    await UploadMedicinesToMongoDBAsync(medicines);
                }
                else
                {
                    Console.WriteLine("Failed to download the Excel file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during data refresh: {ex.Message}");
            }
        }


        public async Task<List<string>> SearchMedicineNamesAsync(string prefix)
        {
            return await _medicineAccess.SearchMedicineNamesAsync(prefix);
        }
    }
}
