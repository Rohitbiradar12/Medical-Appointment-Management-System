

namespace UserManagementService.Model.DTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public string Role {  get; set; } = string.Empty ;
        public string Token { get; set; } = string.Empty;
    }
}
