
using System.Text;
using AppointmentManagementService.Context;
using AppointmentManagementService.Mapper;
using AppointmentManagementService.Repository;
using AppointmentManagementService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentManagementService
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
            builder.Services.AddDbContext<AppointmentContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthenticationConnection"));
            });
            builder.Services.AddAutoMapper(typeof(AppointmentMappingProfile));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:Token"] ?? "")),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });


            #region Repo
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            #endregion

            #region Services
            builder.Services.AddScoped<IAppointmentService,AppointmentService>();   
            builder.Services.AddScoped<ITokenService, TokenService>();
            #endregion

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("DoctorPolicy", policy =>
                    policy.RequireRole("Doctor"));
                options.AddPolicy("PatientPolicy", policy =>
                    policy.RequireRole("Patient"));
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");



            app.MapControllers();

            app.Run();
        }
    }
}
