using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Model.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(8, ErrorMessage = "Minimum length of password should be 8 characters")]
        public string Password { get; set; } = string.Empty;
        [Required]
        public Role Role { get; set; }
    }
}
