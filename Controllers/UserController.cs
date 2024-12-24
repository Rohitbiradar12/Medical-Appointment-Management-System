using Microsoft.AspNetCore.Mvc;
using UserManagementService.Model;
using UserManagementService.Model.DTO;
using UserManagementService.Service;

namespace UserManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
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
                    new { Message = "An unexpected error occurred during login. Please try again later." });
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

    }
}
