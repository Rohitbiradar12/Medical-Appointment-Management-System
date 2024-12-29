using DoctorManagementService.Model;
using DoctorManagementService.Model.DTO;

namespace DoctorManagementService.Service
{
    public interface IAvailabilityService
    {
        Task<AvailabilityDto> GetDoctorAvailabilityAsync(int doctorId);
        Task<IEnumerable<AvailabilityDto>> GetAllAvailabilitiesAsync();
        Task<AvailabilityDto> UpdateAvailabilityAsync(int doctorId, bool isAvailable);
        Task<AvailabilityDto> SetAvailabilityAsync(AvailabilityDto availabilityDto);
        Task DeleteAvailabilityAsync(int id);
    }
}
