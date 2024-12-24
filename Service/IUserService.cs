using UserManagementService.Model.DTO;
using UserManagementService.Model;

namespace UserManagementService.Service
{
    public interface  IUserService
    {
        public Task<UserResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        public Task<UserResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
        public Task<User> GetUser(int  id);
    }
}
