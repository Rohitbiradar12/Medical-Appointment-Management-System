using DoctorManagementService.Model;
using DoctorManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorManagementService.Context
{
    public class DoctorContext : DbContext
    {
        public DoctorContext(DbContextOptions<DoctorContext> options) : base(options) { }  
        
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Availability> Availabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>()
                        .Property(d => d.ConsultationFee)
                        .HasPrecision(18, 2);
        }
    }
}
