using Microsoft.EntityFrameworkCore;
using AppointmentManagementService.Model;

namespace AppointmentManagementService.Context
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .Property(a => a.Status)
                .HasConversion<string>();

           
            modelBuilder.Entity<Appointment>().HasData(
                
                new Appointment
                {
                    Id = 3,
                    PatientId = 3,
                    DoctorId = 3,
                    AppointmentDateTime = new DateTime(2024, 12, 25, 12, 0, 0),
                    Status = AppointmentStatus.Booked,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
               
                new Appointment
                {
                    Id = 5,
                    PatientId = 5,
                    DoctorId = 5,
                    AppointmentDateTime = new DateTime(2024, 12, 25, 14, 0, 0),
                    Status = AppointmentStatus.Booked,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Appointment
                {
                    Id = 6,
                    PatientId = 6,
                    DoctorId = 6,
                    AppointmentDateTime = new DateTime(2024, 12, 25, 15, 0, 0),
                    Status = AppointmentStatus.Booked,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
