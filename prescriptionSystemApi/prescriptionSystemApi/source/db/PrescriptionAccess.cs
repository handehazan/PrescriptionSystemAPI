using Microsoft.EntityFrameworkCore;
using prescriptionSystemApi.context;
using prescriptionSystemApi.model;

namespace prescriptionSystemApi.source.db
{
    public class PrescriptionAccess
    {
        private readonly SqlDbContext _context;
        public PrescriptionAccess(SqlDbContext context)
        {
            _context = context;
        }
        public async Task CreatePrescriptionAsync(Prescription p)
        {
            _context.Prescriptions.Add(p);
            await _context.SaveChangesAsync();
        }

        public async Task AddPrescriptionMedicineAsync(PrescriptionMedicines prescriptionMedicine)
        {
            await _context.PrescriptionsMedicines.AddAsync(prescriptionMedicine);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Prescription>> GetPrescriptionByPatientTCAsync(string patientTC)
        {
            return await _context.Prescriptions.Where(p => p.PatientTC== patientTC).ToListAsync();
        }

        public async Task<List<PrescriptionMedicines>> GetMedicinesByPrescriptionIdAsync(int prescriptionId)
        {
            return await _context.PrescriptionsMedicines.Where(pm => pm.PrescriptionID == prescriptionId).ToListAsync();

        } 

        public async Task MarkPrescriptionAsSubmittedAsync(int prescriptionId)
        {
            var prescription = await _context.Prescriptions.FindAsync(prescriptionId);

            if (prescription == null)
            {
                throw new Exception("Prescription not found");
            }
            prescription.IsSubmitted = true;
            await _context.SaveChangesAsync();
        }





    }
}
