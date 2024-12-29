using AppointmentManagementService.Context;
using AppointmentManagementService.Model;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementService.Repository
{
    
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentContext _context;

        public AppointmentRepository(AppointmentContext context)
        {
            _context = context;
        }
        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            _context.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await GetByIdAsync(id);
            if (appointment == null)
            {
                throw new Exception("User not found");
            }
            appointment.Status = AppointmentStatus.Canceled;
            appointment.UpdatedAt = DateTime.UtcNow;

            
            var result = await _context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            var appointments = _context.Appointments;
            if(appointments.Count()==0)
            {
                throw new Exception("No appointments found");
            }
            return await appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByDoctorAndPatientAsync(int doctorId, int patientId, DateTime appointmentDateTime)
        {
            var appointment = await _context.Appointments.SingleOrDefaultAsync(u => u.DoctorId == doctorId &&
                                                                                     u.PatientId == patientId &&
                                                                                     u.AppointmentDateTime == appointmentDateTime);

            return appointment; 
        }


        public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId)
        {
            var appointments = await _context.Appointments.Where(u => u.DoctorId == doctorId).ToListAsync();
            if (appointments.Count() == 0)
            {
                throw new Exception("No Appointments found");
            }
            return appointments;
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments.SingleOrDefaultAsync(a => a.Id==id);
            if (appointment == null)
            {
                throw new Exception("User not found");
            }
            return appointment;

        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            var appointments = await _context.Appointments.Where(a => a.PatientId == patientId).ToListAsync();
            if (appointments.Count()==0)
            {
                throw new Exception("No appointments found for this patient");
            }

            return appointments;   
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await _context.Set<Appointment>().FindAsync(appointment.Id);
            if (existingAppointment == null)
            {
                throw new Exception("Appointment not found");
            }

            
            existingAppointment.AppointmentDateTime = appointment.AppointmentDateTime;
            existingAppointment.Status = appointment.Status;
            existingAppointment.UpdatedAt = DateTime.UtcNow; 

            
            await _context.SaveChangesAsync();

            return existingAppointment;
        }
    }
}
