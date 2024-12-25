using System.Security.Claims;
using AppointmentManagementService.Model.DTO;

namespace AppointmentManagementService.Service
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDTO> BookAppointmentAsync(BookAppointmentRequestDTO request, ClaimsPrincipal user);
        Task<AppointmentResponseDTO> RescheduleAppointmentAsync(int id, RescheduleAppointmentRequestDTO request, ClaimsPrincipal user);
        Task<bool> CancelAppointmentAsync(int id,ClaimsPrincipal user);
        Task<IEnumerable<AppointmentResponseDTO>> GetAppointmentsByPatientIdAsync(ClaimsPrincipal user);
        Task<IEnumerable<AppointmentResponseDTO>> GetAppointmentsByDoctorIdAsync(ClaimsPrincipal user);
    }
}
