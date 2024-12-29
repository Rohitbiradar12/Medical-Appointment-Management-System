namespace DoctorManagementService.Controllers
{
    using DoctorManagementService.Model.DTO;
    using DoctorManagementService.Service;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly ILogger<AvailabilityController> _logger;

        public AvailabilityController(IAvailabilityService availabilityService, ILogger<AvailabilityController> logger)
        {
            _availabilityService = availabilityService;
            _logger = logger;
        }

        
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetAvailabilityByDoctorId(int doctorId)
        {
            try
            {
                var availability = await _availabilityService.GetDoctorAvailabilityAsync(doctorId);
                return Ok(availability);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Doctor ID {doctorId} not found or no availability found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching availability for Doctor ID {doctorId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAllAvailabilities()
        {
            try
            {
                var availabilities = await _availabilityService.GetAllAvailabilitiesAsync();
                return Ok(availabilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all availabilities.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        
        [HttpPut("{doctorId}")]
        public async Task<IActionResult> UpdateAvailability(int doctorId, [FromBody] bool isAvailable)
        {
            try
            {
                var updatedAvailability = await _availabilityService.UpdateAvailabilityAsync(doctorId, isAvailable);
                return Ok(updatedAvailability);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Doctor ID {doctorId} not found for update.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating availability for Doctor ID {doctorId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> SetAvailability([FromBody] AvailabilityDto availabilityDto)
        {
            try
            {
                var createdAvailability = await _availabilityService.SetAvailabilityAsync(availabilityDto);
                return CreatedAtAction(nameof(GetAvailabilityByDoctorId), new { doctorId = createdAvailability.DoctorId }, createdAvailability);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting new availability.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            try
            {
                await _availabilityService.DeleteAvailabilityAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Availability ID {id} not found for deletion.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting availability with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }

}
