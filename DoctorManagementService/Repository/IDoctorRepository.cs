using DoctorManagementService.Models;

namespace DoctorManagementService.Repository
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetDoctorByIdAsync(int id);
        Task<Doctor> UpdateDoctorAsync(Doctor doctor);
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<Doctor> AddDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(int id);
        Task<bool> DoesDoctorExistAsync(int doctorId);
        Task SaveAsync();
    }
}
