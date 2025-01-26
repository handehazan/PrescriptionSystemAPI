namespace prescriptionSystemApi.model.dto
{
    public class PrescriptionDto
    {
    }
    public class SubmitPrescriptionDto
    {
        public int prescriptionId { get; set; }
        public string pharmacyName { get; set; }
        public List<String> MedicinesGiven {  get; set; }

    }

    public class SubmitPrescriptionResponseDto
    {
        public string PharmacyName { get; set; }
        public int PrescriptionId { get; set; }
        public List<string> MissingMedicines { get; set; }
    }
}
