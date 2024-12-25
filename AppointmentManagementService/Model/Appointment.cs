namespace AppointmentManagementService.Model
{   
            public class Appointment
        {
            public int Id { get; set; }            
            public int PatientId { get; set; }
            public int DoctorId { get; set; }
            public DateTime AppointmentDateTime { get; set; }
            public AppointmentStatus Status { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        }

        
        public enum AppointmentStatus
        {
            Booked,
            Rescheduled,
            Canceled
        }
 }
