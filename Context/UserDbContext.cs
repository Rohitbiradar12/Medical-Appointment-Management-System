
using Microsoft.EntityFrameworkCore;
using UserManagementService.Model;

namespace UserManagementService.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationship between User and Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.role) // Ensure proper casing
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // Seed data for Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Doctor" },
                new Role { RoleId = 3, RoleName = "Patient" }
            );
        }
    }
}
