using AppointmentManagementService.Model;

namespace AppointmentManagementService.Repository
{
    public interface IAppointmentRepository 
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment> GetByIdAsync(int  id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(int id);
        Task<Appointment> GetAppointmentByDoctorAndPatientAsync(int doctorId, int patientId, DateTime appointmentDateTime);




    }
}
