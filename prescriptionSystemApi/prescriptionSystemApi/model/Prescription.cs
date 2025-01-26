using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prescriptionSystemApi.model
{
    [Table("Prescription")]
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionId { get; set; } // Primary key

        [Required]
        [MaxLength(11)]
        public string PatientTC { get; set; } // Patient's TC number

        [Required]
        public DateTime VisitDate { get; set; } = DateTime.UtcNow; // Date of the visit, defaults to the current date

        [Required]
        [MaxLength(50)]
        public string DoctorId { get; set; } // Doctor's ID
        public bool IsSubmitted { get; set; } = false; // Flag to check if the prescription is submitted
    }
}
