using prescriptionSystemApi.model;

namespace prescriptionSystemApi.source.svc
{
    public interface IMedicineService
    {
        public Task<List<Medicine>> GetAllMedicinesAsync();
        public Task<string> DownoadMedicineExcel();
        public List<Medicine> ParseExcelFile(string filePath);
        public Task RefreshMedicineDataAsync();
        public Task<List<string>> SearchMedicineNamesAsync(string prefix);

    }
}
