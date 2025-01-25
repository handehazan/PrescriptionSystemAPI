using prescriptionSystemApi.model.dto;
using prescriptionSystemApi.model;
using prescriptionSystemApi.source.db;

namespace prescriptionSystemApi.source.svc
{
    public interface IPrescriptionService
    {
        public Task<Prescription> CreatePrescriptionAsync(CreatePrescriptionDto dto);
        public Task<List<Prescription>> GetPrescriptionByPatientTCAsync(string patientTC);
        public Task<List<PrescriptionMedicines>> GetMedicinesByPrescriptionIdAsync(int prescriptionId);
        public Task<SubmitPrescriptionResponseDto> SubmitPrescriptionAsync(SubmitPrescriptionDto dto);

    }
}
