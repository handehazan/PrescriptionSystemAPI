using prescriptionSystemApi.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prescriptionSystemApi.source.db
{
    [Table("PrescriptionMedicines")]
    public class PrescriptionMedicines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionMedicineID { get; set; }

        [Required]
        public int PrescriptionID { get; set; }

        [MaxLength(200)]
        public string MedicineName { get; set; }

        // Navigation Property (Optional, for Entity Framework)
        [ForeignKey("PrescriptionID")]
        public Prescription Prescription { get; set; }

    }
}
