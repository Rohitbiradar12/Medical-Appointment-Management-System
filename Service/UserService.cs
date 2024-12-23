using UserManagementService.Model.DTO;
using UserManagementService.Repository;
using UserManagementService.Model;
using System.Security.Cryptography;
using System.Text;

namespace UserManagementService.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _repository;
        private readonly ILogger<UserService> _logger;
        private readonly ITokenService _tokenService;

        public UserService(IRepository<int,User> repository, ILogger<UserService> logger, ITokenService tokenService)
        {
            _repository = repository;
            _logger = logger;
            _tokenService = tokenService;
        }
        public async Task<UserResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user =  await _repository.Get(loginRequestDTO.Email);
            if (user == null)
            {
                _logger.LogCritical("Login attempt failed, Invalid user email");
                throw new Exception("user not found");
            }
            HMACSHA256 hmac = new HMACSHA256(user.Key);
            var password = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequestDTO.Password));
            for(int i = 0; i < password.Length; i++)
            {
                if(password[i] != user.Password[i])
                {
                    _logger.LogWarning("User login attempt failed, Invalid password");
                    throw new Exception("Invalid password");
                }
            }
            var userResponse = new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email, 
            };
            userResponse.Token = await _tokenService.GenerateToken(userResponse);
            return userResponse;

        }

        public async Task<UserResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            HMACSHA256 hmac = new HMACSHA256();
            User user = new User();
            user.FirstName = registerRequestDTO.FirstName;
            user.LastName = registerRequestDTO.LastName;
            user.Email = registerRequestDTO.Email;
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerRequestDTO.Password));
            user.role = registerRequestDTO.Role;
            user.Key = hmac.Key;
            var result = _repository.Add(user);
            if(result == null)
            {
                _logger.LogWarning("User creation failed");
                throw new Exception("User entity cannot be empty");
            }
            var response = new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            
            return response;
        }
    }
}
