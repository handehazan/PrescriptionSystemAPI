namespace prescriptionSystemApi.model
{
    public class NotificationMessage
    {
        public string PharmacyName { get; set; }
        public int PrescriptionId { get; set; }
        public List<string> MissingMedicines { get; set; }
    }

    public class PrescriptionSummary
    {
        public int PrescriptionId { get; set; }
        public List<string> MissingMedicines { get; set; }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
    }
}
