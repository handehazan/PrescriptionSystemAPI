namespace prescriptionSystemApi.model.dto
{
    public class CreatePrescriptionDto
    {
        public string PatientTC { get; set; }
        public DateTime VisitDate { get; set; }
        public string DoctorId { get; set; }
        public List<CreatePrescriptionMedicineDto> Medicines { get; set; }
    }
    public class CreatePrescriptionMedicineDto
    {
        public string MedicineName { get; set; }  // Only the medicine name
    }

    public class ReturnPrescriptionDto
    {
        public string Id { get; set; }
    }

}
