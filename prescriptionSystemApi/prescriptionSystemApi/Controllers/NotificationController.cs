using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prescriptionSystemApi.model;
using prescriptionSystemApi.source.svc;

namespace prescriptionSystemApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _inotificationService;

        public NotificationController(INotificationService inotificationService)
        {
            _inotificationService = inotificationService;
        }

        /// <summary>
        /// Start listening to messages on the RabbitMQ queue.
        /// </summary>
        [HttpGet("start-listening")]
        public IActionResult StartListening()
        {
            try
            {
                _inotificationService.StartListening();
                return Ok("Started listening to RabbitMQ messages.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("send-emails")]
        public IActionResult SendEmails()
        {
            try
            {
                _inotificationService.SendEmails();
                return Ok("Emails have been sent successfully. Check logs for details.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending emails: {ex.Message}");
            }
        }

        [HttpPost("process-and-sendmail")]
        public async Task<IActionResult> ProcessAndSendEmails()
        {
            try
            {
                await _inotificationService.ProcessAndSendEmailsAsync();
                return Ok("Messages processed and emails sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
