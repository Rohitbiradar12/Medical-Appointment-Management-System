using System.ComponentModel.DataAnnotations;

namespace DoctorManagementService.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Speciality { get; set; } = string.Empty ;

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        public decimal ConsultationFee { get; set; }
    }
}
