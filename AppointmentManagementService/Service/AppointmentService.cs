﻿using System.Security.Claims;
using AppointmentManagementService.Model;
using AppointmentManagementService.Model.DTO;
using AppointmentManagementService.Repository;
using System.Net.Http;
using AutoMapper;

namespace AppointmentManagementService.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly ITokenService tokenService;
        private readonly ILogger<AppointmentService> logger;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;

        public AppointmentService(IAppointmentRepository appointmentRepository, ILogger<AppointmentService> logger, IMapper mapper, ITokenService tokenService, HttpClient httpClient)
        {
            this.appointmentRepository = appointmentRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.httpClient = httpClient;
        }

        public async Task<AppointmentResponseDTO> BookAppointmentAsync(BookAppointmentRequestDTO request, ClaimsPrincipal user)
        {
            var userId = tokenService.GetUserIdFromToken(user);
            var userRole = tokenService.GetUserRoleFromToken(user);

            logger.LogInformation($"Booking appointment for UserID: {userId}, Role: {userRole}");


            if (userRole != "Patient" || userId != request.PatientId)
            {
                logger.LogWarning($"Unauthorized access attempt by UserID: {userId}, Role: {userRole}");
                throw new UnauthorizedAccessException("You are not authorized to book this appointment.");
            }

            var isDoctorAvailable = await CheckDoctorAvailabilityAsync(request.DoctorId);
            if (!isDoctorAvailable)
            {
                logger.LogWarning($"Doctor with ID {request.DoctorId} is not available for appointment at {request.AppointmentDateTime}.");
                throw new InvalidOperationException("Doctor is not available at the requested time.");
            }

            var existingAppointment = await appointmentRepository.GetAppointmentByDoctorAndPatientAsync(request.DoctorId, request.PatientId, request.AppointmentDateTime);
            if (existingAppointment != null)
            {
                logger.LogWarning($"Appointment already exists for DoctorID: {request.DoctorId}, PatientID: {request.PatientId} at {request.AppointmentDateTime}");
                throw new InvalidOperationException("An appointment already exists for this doctor and time.");
            }

            var appointment = mapper.Map<Appointment>(request);
            appointment.Status = AppointmentStatus.Booked;
            appointment.CreatedAt = DateTime.UtcNow;

            var createdAppointment = await appointmentRepository.AddAsync(appointment);

            logger.LogInformation($"Appointment successfully booked with ID: {createdAppointment.Id}");

            return mapper.Map<AppointmentResponseDTO>(createdAppointment);
        }



        public async Task<bool> CancelAppointmentAsync(int id, ClaimsPrincipal user)
        {
            var userId = tokenService.GetUserIdFromToken(user);
            var userRole = tokenService.GetUserRoleFromToken(user);

            logger.LogInformation($"Canceling appointment with ID: {id} for UserID: {userId}, Role: {userRole}");

            var appointment = await appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                logger.LogWarning($"Appointment with ID: {id} not found.");
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (userRole != "Patient" || userId != appointment.PatientId)
            {
                logger.LogWarning($"Unauthorized cancel attempt by UserID: {userId}, Role: {userRole} for AppointmentID: {id}");
                throw new UnauthorizedAccessException("You are not authorized to cancel this appointment.");
            }

            return await appointmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AppointmentResponseDTO>> GetAppointmentsByDoctorIdAsync(ClaimsPrincipal user)
        {
            var userId = tokenService.GetUserIdFromToken(user);
            var userRole = tokenService.GetUserRoleFromToken(user);

            logger.LogInformation($"Fetching appointments for DoctorID: {userId}, Role: {userRole}");

            if (userRole != "Doctor")
            {
                logger.LogWarning($"Unauthorized access attempt by UserID: {userId}, Role: {userRole}");
                throw new UnauthorizedAccessException("You are not authorized to view appointments.");
            }

            var appointments = await appointmentRepository.GetByDoctorIdAsync(userId);

            logger.LogInformation($"Found {appointments.Count()} appointments for DoctorID: {userId}");

            return mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments);
        }

        public async Task<IEnumerable<AppointmentResponseDTO>> GetAppointmentsByPatientIdAsync(ClaimsPrincipal user)
        {
            var userId = tokenService.GetUserIdFromToken(user);
            var userRole = tokenService.GetUserRoleFromToken(user);

            logger.LogInformation($"Fetching appointments for PatientID: {userId}, Role: {userRole}");

            if (userRole != "Patient")
            {
                logger.LogWarning($"Unauthorized access attempt by UserID: {userId}, Role: {userRole}");
                throw new UnauthorizedAccessException("You are not authorized to view appointments.");
            }

            var appointments = await appointmentRepository.GetByPatientIdAsync(userId);

            logger.LogInformation($"Found {appointments.Count()} appointments for PatientID: {userId}");

            return mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments);
        }

        public async Task<AppointmentResponseDTO> RescheduleAppointmentAsync(int id, RescheduleAppointmentRequestDTO request, ClaimsPrincipal user)
        {
            var userId = tokenService.GetUserIdFromToken(user);
            var userRole = tokenService.GetUserRoleFromToken(user);

            logger.LogInformation($"Rescheduling appointment with ID: {id} for UserID: {userId}, Role: {userRole}");

            var appointment = await appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                logger.LogWarning($"Appointment with ID: {id} not found.");
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (userRole != "Patient" || userId != appointment.PatientId)
            {
                logger.LogWarning($"Unauthorized reschedule attempt by UserID: {userId}, Role: {userRole} for AppointmentID: {id}");
                throw new UnauthorizedAccessException("You are not authorized to reschedule this appointment.");
            }

            appointment.AppointmentDateTime = request.NewAppointmentDateTime;
            appointment.Status = AppointmentStatus.Rescheduled;
            appointment.UpdatedAt = DateTime.UtcNow;

            var updatedAppointment = await appointmentRepository.UpdateAsync(appointment);

            logger.LogInformation($"Appointment with ID: {id} successfully rescheduled.");

            return mapper.Map<AppointmentResponseDTO>(updatedAppointment);
        }

        public async Task<bool> CheckDoctorAvailabilityAsync(int doctorId)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5223/api/Availability/{doctorId}");
                response.EnsureSuccessStatusCode();

                var availabilityDto = await response.Content.ReadFromJsonAsync<AvailabilityDto>();

                if (availabilityDto == null || !availabilityDto.IsAvailable)
                {
                    logger.LogWarning($"Doctor with ID {doctorId} is not available.");
                    return false;
                }

                logger.LogInformation($"Doctor with ID {doctorId} is available.");
                return true;
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, $"Error occurred while checking availability for Doctor ID: {doctorId}");
                throw new ApplicationException("Unable to check doctor availability. Please try again later.", ex);
            }
        }


    }
}
