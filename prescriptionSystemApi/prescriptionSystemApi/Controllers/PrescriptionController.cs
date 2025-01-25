using Microsoft.AspNetCore.Mvc;
using prescriptionSystemApi.model.dto;
using prescriptionSystemApi.source.svc;

namespace prescriptionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        public PrescriptionController(PrescriptionService service)
        {
            _service = service;
        }


        [HttpPost("CreatePrescription")]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto dto)
        {
            if (dto == null || dto.Medicines == null || dto.Medicines.Count == 0)
            {
                return BadRequest("Invalid input: Medicines are required.");
            }

            // Create the prescription
            var prescription = await _service.CreatePrescriptionAsync(dto);

            // Return the created prescription
            return CreatedAtAction(nameof(CreatePrescription), new { id = prescription.PrescriptionId }, prescription);
        }

        [HttpGet("{patientTC}")]
        public async Task<IActionResult> GetPrescriptionByPatientTC(string patientTC)
        {
            var prescription = await _service.GetPrescriptionByPatientTCAsync(patientTC);
            if (prescription == null) {
                return NotFound(new { Message = "Prescription not found for the given patient" });
            }

            return Ok(prescription);
        }

        [HttpGet("medicines/{prescriptionId}")]
        public async Task<IActionResult> GetMedicinesByPrescriptionIdAsync(int prescriptionId)
        {
            var medicines = await _service.GetMedicinesByPrescriptionIdAsync(prescriptionId);
            if (medicines == null) 
            {
                return NotFound(new { Message = "No medicines found for the given PrescriptionId." });
            }

            var medicineDtos = medicines.Select(m => new ShowMedicineDto { MedicineName = m.MedicineName}).ToList();
            return Ok(medicineDtos);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitPrescription([FromBody] SubmitPrescriptionDto dto)
        {
            try
            {
                var response = await _service.SubmitPrescriptionAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { MEssage = ex.Message });
            }

        }
    }
}
