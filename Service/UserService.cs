using UserManagementService.Model.DTO;
using UserManagementService.Repository;
using UserManagementService.Model;
using System.Security.Cryptography;
using System.Text;


namespace UserManagementService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IRepository<int, Role> _roleRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repository, ILogger<UserService> logger, ITokenService tokenService, IRepository<int, Role> roleRepo, IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _tokenService = tokenService;
            _roleRepository = roleRepo;
            _configuration = configuration;
        }
        public async Task<UserResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _repository.Get(loginRequestDTO.Email);
            if (user == null)
            {
                _logger.LogCritical("Login attempt failed, Invalid user email");
                throw new Exception("user not found");
            }
            //if (user.role == null || string.IsNullOrEmpty(user.role.RoleName))
            //{
            //    _logger.LogError("Role not found for the user.");
            //    throw new Exception("User role is invalid or not set.");
            //}
            if (!user.IsEmailVerified)
            {
                _logger.LogWarning($"Login attempt failed for user {loginRequestDTO.Email}, email not verified");
                throw new Exception("Please verify your email before logging in.");
            }

            HMACSHA256 hmac = new HMACSHA256(user.Key);
            var password = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequestDTO.Password));
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != user.Password[i])
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
                Role = user.role.RoleName
            };
            userResponse.Token = await _tokenService.GenerateToken(userResponse);
            return userResponse;

        }

        public async Task<UserResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            
            HMACSHA256 hmac = new HMACSHA256();
            var role = await _roleRepository.Get(registerRequestDTO.Role);
            if (role == null)
            {
                _logger.LogError($"Role '{registerRequestDTO.Role}' not found.");
                throw new Exception("Invalid role specified.");
            }

            
            var user = new User
            {
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                Email = registerRequestDTO.Email,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerRequestDTO.Password)),
                Key = hmac.Key,
                role = role,
                // Generate Email Verification Token 
                EmailVerificationToken = Guid.NewGuid().ToString(),
                IsEmailVerified = false 
            };

            try
            {
                
                var result = await _repository.Add(user);
                if (result == null)
                {
                    _logger.LogWarning("User creation failed");
                    throw new Exception("Failed to create user in the database.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating user: {ex.Message}");
                throw new Exception("Unexpected error during user registration.");
            }

            // Send Email Verification (Pass the user object with the verification token)
            var emailService = new EmailService(_configuration);
            await emailService.SendEmailVerificationAsync(user);

            
            var userResponse = new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = role.RoleName,
                
            };

            userResponse.Token = await _tokenService.GenerateToken(userResponse);

            
            return userResponse;
        }


        public async Task<User> GetUser(int id)
        {
            var user = await _repository.Get(id);
            if (user == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("User doesnt exist");
            }
            
            return user;
        }

        public async Task SeedAdminUserAsync()
        {
            var adminEmail = _configuration["EmailSettings:SenderEmail"];
            var adminPassword = _configuration["AdminSettings:DefaultPassword"];  

            var existingAdmin = await _repository.Get(adminEmail);
            if (existingAdmin == null)
            {
                HMACSHA256 hmac = new HMACSHA256();
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = adminEmail,
                    Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(adminPassword)),
                    Key = hmac.Key,
                    IsEmailVerified = true, 
                    EmailVerificationToken = null,
                    role = await _roleRepository.Get("Admin")
                };
                await _repository.Add(adminUser);
            }
        }




    }
}