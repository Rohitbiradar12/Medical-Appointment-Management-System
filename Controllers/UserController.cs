using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Model;
using UserManagementService.Model.DTO;
using UserManagementService.Repository;
using UserManagementService.Service;

namespace UserManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _userService.Login(loginRequestDTO);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                var user = await _userService.Register(registerRequestDTO);
                if (user == null)
                {
                    return Unauthorized();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred during registration. Please try again later." });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);
                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] string token)
        {
            _logger.LogInformation("Received token for verification: {Token}", token);

            var user = await _userRepository.GetUserByEmailVerificationTokenAsync(token);

            if (user == null)
            {
                _logger.LogWarning("Invalid or expired verification token.");
                return BadRequest("Invalid or expired verification token.");
            }

            _logger.LogInformation("User found for token verification: {UserId}", user.Id);

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null; 

            try
            {
                _logger.LogInformation("Attempting to update user with ID: {UserId}");

                await _userRepository.Update(user);

                _logger.LogInformation("User with ID: {UserId} successfully updated.");

                return Ok("Email successfully verified. You can now log in.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error verifying email: {ex.Message}. StackTrace: {ex.StackTrace}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
