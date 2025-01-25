using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prescriptionSystemApi.model;
using prescriptionSystemApi.source.svc;

namespace prescriptionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly MedicineService _medicineService;
        public MedicineController(MedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
     [FromQuery] int page = 1,       // Default to page 1
     [FromQuery] int pageSize = 10)  // Default to 10 items per page
        {
            // Validate input
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            // Fetch all medicines from the service layer
            var allMedicines = await _medicineService.GetAllMedicinesAsync();

            // Calculate pagination
            int totalCount = allMedicines.Count;
            int skip = (page - 1) * pageSize;

            // Apply pagination in-memory
            var paginatedMedicines = allMedicines
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // Return the paginated response with metadata
            return Ok(new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Data = paginatedMedicines
            });
        }

        [HttpGet("SearchMedicine")]
        public async Task<IActionResult> SearchMedicines([FromQuery] string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return BadRequest("Search term is empty.");
            }

            var medicationNames = await _medicineService.SearchMedicineNamesAsync(prefix);
            return Ok(new { medicationNames });
        }

        [HttpGet("dowload-excel")]
        public async Task<IActionResult> DownloadExcel()
        {
            string result = await _medicineService.DownoadMedicineExcel();
            return Ok(result);
        }

        [HttpGet("test-parse")]
        public async Task<IActionResult> TestParseExcel([FromQuery] int page = 1, [FromQuery] int pageSize = 4)
        {
            // Step 1: Download the Excel file
            string filePath = await _medicineService.DownoadMedicineExcel();

            // Check if the file was downloaded successfully
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest("Failed to download the Excel file.");
            }

            // Step 2: Parse the Excel file
            var medicines = _medicineService.ParseExcelFile(filePath);

            // Apply pagination
            var paginatedMedicines = medicines
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return the paginated data as a response
            return Ok(new
            {
                TotalCount = medicines.Count,
                Page = page,
                PageSize = pageSize,
                Data = paginatedMedicines
            });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshMedicines()
        {
            await _medicineService.RefreshMedicineDataAsync();
            return Ok("Medicine data refreshed successfully");
        }
    }
}
