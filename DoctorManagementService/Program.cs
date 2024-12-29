
using DoctorManagementService.Context;
using DoctorManagementService.Mapper;
using DoctorManagementService.Repository;
using DoctorManagementService.Service;
using Microsoft.EntityFrameworkCore;

namespace DoctorManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Repositories
            builder.Services.AddScoped<IDoctorRepository,DoctorRepository>();
            builder.Services.AddScoped<IAvailabilityRepository,AvailabilityRepository>();
            #endregion

            #region Services
            builder.Services.AddScoped<IAvailabilityService,AvailabiltyService>();
            builder.Services.AddScoped<IDoctorService,DoctorService>();
            #endregion

            #region Mapper
            builder.Services.AddAutoMapper(typeof(DoctorProfile));
            builder.Services.AddAutoMapper(typeof(AvailabilityProfile));
            #endregion

            builder.Services.AddDbContext<DoctorContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
