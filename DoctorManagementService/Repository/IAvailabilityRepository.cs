using DoctorManagementService.Model;

namespace DoctorManagementService.Repository
{
    public interface IAvailabilityRepository
    {
        Task<Availability> GetAvailabilityByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync();
        Task<Availability> UpdateAvailabilityAsync(Availability availability);
        Task<Availability> AddAvailabilityAsync(Availability availability);
        Task DeleteAvailabilityAsync(int id);
        Task SaveAsync();
    }

}
