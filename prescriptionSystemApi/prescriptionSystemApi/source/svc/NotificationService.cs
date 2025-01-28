using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using prescriptionSystemApi.model;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
namespace prescriptionSystemApi.source.svc
{
    public class NotificationService : INotificationService
    {
        private readonly RabbitmqService _rabbitmqService;
        private readonly Dictionary<string, List<PrescriptionSummary>> _pharmacyPrescriptions;
        private readonly EmailSettings _emailSettings;
        private readonly object _lock = new object(); // To handle concurrency
        public NotificationService(RabbitmqService rabbitmqService, IConfiguration configuration)
        {
            _rabbitmqService = rabbitmqService;
            _pharmacyPrescriptions = new Dictionary<string, List<PrescriptionSummary>>();
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        }

        public void StartListening()
        {
            Console.WriteLine("Started listening to the RabbitMQ queue...");
            _rabbitmqService.ConsumeMessage("missing-medicine", async (message) =>
            {
                await Task.Run(() => ProcessMessage(message)); // Ensure async processing
            });
        }

        public void ProcessMessage(string message)
        {
            lock (_lock)
            {
                // Deserialize the message
                var notificationMessage = JsonConvert.DeserializeObject<NotificationMessage>(message);

                if (notificationMessage != null)
                {
                    // Group data by pharmacy name
                    if (!_pharmacyPrescriptions.ContainsKey(notificationMessage.PharmacyName))
                    {
                        _pharmacyPrescriptions[notificationMessage.PharmacyName] = new List<PrescriptionSummary>();
                    }

                    // Add prescription details to the pharmacy
                    _pharmacyPrescriptions[notificationMessage.PharmacyName].Add(new PrescriptionSummary
                    {
                        PrescriptionId = notificationMessage.PrescriptionId,
                        MissingMedicines = notificationMessage.MissingMedicines
                    });

                    // Log dictionary contents for debugging
                    Console.WriteLine("Current Grouped Data:");
                    foreach (var pharmacy in _pharmacyPrescriptions)
                    {
                        Console.WriteLine($"Pharmacy: {pharmacy.Key}");
                        foreach (var prescription in pharmacy.Value)
                        {
                            Console.WriteLine($"  Pres Id: {prescription.PrescriptionId} - Missing: {string.Join(", ", prescription.MissingMedicines)}");
                        }
                    }
                    Console.WriteLine($"Processed Prescription ID: {notificationMessage.PrescriptionId} for Pharmacy: {notificationMessage.PharmacyName}");
                }
            }
        }
        public async Task ProcessAndSendEmailsAsync()
        {
            Console.WriteLine("Starting to process messages...");
            // Start consuming messages from the queue
            StartListening();

            // Wait for a fixed duration (e.g., 30 seconds) to allow message consumption
            await Task.Delay(TimeSpan.FromSeconds(30));

            Console.WriteLine("Finished processing messages. Sending emails...");

            // Send grouped emails
            SendEmails();
        }
        

        public void SendEmails()
        {
            foreach (var pharmacy in _pharmacyPrescriptions)
            {
                var pharmacyName = pharmacy.Key;
                var prescriptions = pharmacy.Value;

                // Look up pharmacy email from UserConstants
                var pharmacyEmail = UserConstants.Users
                    .Where(u => u.Name.Equals(pharmacyName, StringComparison.OrdinalIgnoreCase) && u.Role == "pharmacy")
                    .Select(u => u.Email)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(pharmacyEmail))
                {
                    Console.WriteLine($"No email found for pharmacy: {pharmacyName}");
                    continue; // Skip this pharmacy
                }

                // Generate email subject and body
                var subject = $"Daily Prescription Report for {pharmacyName}";
                var body = $"Dear {pharmacyName},\n\n" +
                           $"You have {prescriptions.Count} incomplete prescriptions today:\n";

                foreach (var prescription in prescriptions)
                {
                    body += $"- Pres Id: {prescription.PrescriptionId} - Missing: {string.Join(", ", prescription.MissingMedicines)}\n";
                }

                // Send the email
                try
                {
                    var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                    {
                        Port = _emailSettings.SmtpPort,
                        Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                        EnableSsl = true
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.SenderEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(pharmacyEmail);

                    smtpClient.Send(mailMessage);
                    Console.WriteLine($"Email sent successfully to {pharmacyEmail}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {pharmacyEmail}: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }

            // Clear the data after sending emails
            _pharmacyPrescriptions.Clear();
        }
        public Dictionary<string, List<PrescriptionSummary>> GetGroupedPharmacyPrescriptions()
        {
            return _pharmacyPrescriptions;
        }
    }
}