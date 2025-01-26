using prescriptionSystemApi.model;

namespace prescriptionSystemApi.source.svc
{
    public interface INotificationService
    {
        public void StartListening();
        public void ProcessMessage(string message);
        public Dictionary<string, List<PrescriptionSummary>> GetGroupedPharmacyPrescriptions();
        public void SendEmails();
        public Task ProcessAndSendEmailsAsync();







    }
}
