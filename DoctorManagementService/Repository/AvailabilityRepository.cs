using DoctorManagementService.Context;
using DoctorManagementService.Model;
using DoctorManagementService.CustomException;
using Microsoft.EntityFrameworkCore;

namespace DoctorManagementService.Repository
{
    public class AvailabilityRepository : IAvailabilityRepository
    {

        private readonly DoctorContext _context;
        private readonly ILogger<AvailabilityRepository> _logger;   

        public AvailabilityRepository(DoctorContext context,ILogger<AvailabilityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Availability> AddAvailabilityAsync(Availability availability)
        {
            try
            {
                _context.Availabilities.Add(availability);
                await _context.SaveChangesAsync();
                return availability;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding new availability.");
                throw new DataAccessException("Error occurred while adding availability.", ex);
            }
        }

        public async Task DeleteAvailabilityAsync(int id)
        {
            try
            {
                var avail = await _context.Availabilities.FindAsync(id);
                if (avail != null)
                {
                    _context.Availabilities.Remove(avail);
                    await _context.SaveChangesAsync();
                    
                }

            }catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting availability with ID: {id}");
                throw new DataAccessException("Error occurred while deleting availability.", ex);
            }
        }

        public async Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync()
        {
            try
            {
                return await _context.Availabilities.ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all availabilities.");
                throw new DataAccessException("Error occurred while fetching all availabilities.", ex);
            }
        }
       
        public async Task<Availability> GetAvailabilityByDoctorIdAsync(int doctorId)
        {
            try
            {
                var avail = await _context.Availabilities.SingleOrDefaultAsync(a => a.DoctorId == doctorId);
                return avail;

            }catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching availability for DoctorId: {doctorId}");
                throw new DataAccessException("Error occurred while fetching availability by DoctorId.", ex);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database.");
                throw new DataAccessException("Error occurred while saving changes.", ex);
            }
        }

        public async Task<Availability> UpdateAvailabilityAsync(Availability availability)
        {
            try
            {
                _context.Availabilities.Update(availability);
                await _context.SaveChangesAsync();
                return availability;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating availability with ID: {availability.Id}");
                throw new DataAccessException("Error occurred while updating availability.", ex);
            }
        }
    }
}
