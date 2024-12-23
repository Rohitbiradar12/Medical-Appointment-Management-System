using UserManagementService.Model.DTO;

namespace UserManagementService.Service
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(UserResponseDTO responseDTO);
    }
}
