using DoctorManagementService.Model.DTO;

namespace DoctorManagementService.Service
{
    public interface IDoctorService
    {
        Task<DoctorProfileDTO> GetDoctorProfileAsync(int id);
        Task UpdateDoctorProfileAsync(int id, DoctorProfileDTO profileDTO);
        Task<IEnumerable<DoctorProfileDTO>> GetAllDoctorsAsync();
        Task<DoctorProfileDTO> AddDoctorAsync(DoctorProfileDTO profileDTO);
        Task DeleteDoctorAsync(int id);
    }
}
