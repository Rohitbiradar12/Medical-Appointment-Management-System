using DoctorManagementService.Model.DTO;
using DoctorManagementService.Service;
using Microsoft.AspNetCore.Mvc;

namespace DoctorManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(IDoctorService doctorService, ILogger<DoctorController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            try
            {
                var doctorProfile = await _doctorService.GetDoctorProfileAsync(id);
                return Ok(doctorProfile);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Doctor with ID {Id} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving doctor with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctorsAsync();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all doctors.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorProfileDTO doctorProfile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                var addedDoctor = await _doctorService.AddDoctorAsync(doctorProfile);
                return Ok(addedDoctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new doctor.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorProfileDTO doctorProfile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                await _doctorService.UpdateDoctorProfileAsync(id, doctorProfile);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Doctor with ID {Id} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating doctor with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                await _doctorService.DeleteDoctorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Doctor with ID {Id} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting doctor with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
