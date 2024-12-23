using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Model.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email is mandatory")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; } 
    }
}
