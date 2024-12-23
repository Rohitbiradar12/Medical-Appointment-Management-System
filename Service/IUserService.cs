using UserManagementService.Model.DTO;

namespace UserManagementService.Service
{
    public interface  IUserService
    {
        public Task<UserResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        public Task<UserResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
