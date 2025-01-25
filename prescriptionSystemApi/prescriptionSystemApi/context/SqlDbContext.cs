using Microsoft.EntityFrameworkCore;
using prescriptionSystemApi.model;
using prescriptionSystemApi.source.db;

namespace prescriptionSystemApi.context
{
    public class SqlDbContext :DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicines> PrescriptionsMedicines { get; set; }

    }
}
