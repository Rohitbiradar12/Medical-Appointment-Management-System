using AutoMapper;
using DoctorManagementService.Model;
using DoctorManagementService.Model.DTO;
using DoctorManagementService.Repository;

namespace DoctorManagementService.Service
{

    public class AvailabiltyService : IAvailabilityService
    {
        private readonly IAvailabilityRepository repository;
        private readonly ILogger<AvailabiltyService> logger;
        private readonly IMapper mapper;
        private readonly IDoctorRepository doctorRepository;

        public AvailabiltyService(
            IAvailabilityRepository repository,
            ILogger<AvailabiltyService> logger,
            IMapper mapper,
            IDoctorRepository doctorRepository)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
            this.doctorRepository = doctorRepository;
        }

        private async Task EnsureDoctorExistsAsync(int doctorId)
        {
            var doctorExists = await doctorRepository.DoesDoctorExistAsync(doctorId);
            if (!doctorExists)
            {
                logger.LogWarning($"Doctor with ID {doctorId} does not exist.");
                throw new KeyNotFoundException($"Doctor with ID {doctorId} not found.");
            }
        }

        public async Task DeleteAvailabilityAsync(int id)
        {
            try
            {
                await repository.DeleteAvailabilityAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning($"Availability with ID {id} not found for deletion.");
                throw new KeyNotFoundException($"Availability with ID {id} not found.", ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred while deleting availability with ID: {id}");
                throw new ApplicationException("An error occurred while deleting availability.", ex);
            }
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAllAvailabilitiesAsync()
        {
            try
            {
                var availabilities = await repository.GetAllAvailabilitiesAsync();
                return mapper.Map<IEnumerable<AvailabilityDto>>(availabilities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching all availabilities.");
                throw new ApplicationException("An error occurred while fetching availabilities.", ex);
            }
        }

        public async Task<AvailabilityDto> GetDoctorAvailabilityAsync(int doctorId)
        {
            logger.LogInformation($"Fetching availability for Doctor ID: {doctorId}");

            await EnsureDoctorExistsAsync(doctorId);

            var availability = await repository.GetAvailabilityByDoctorIdAsync(doctorId);
            if (availability == null)
            {
                logger.LogWarning($"No availability found for Doctor ID: {doctorId}");
                throw new KeyNotFoundException($"No availability found for Doctor ID: {doctorId}");
            }

            logger.LogInformation($"Availability found for Doctor ID: {doctorId}");
            return mapper.Map<AvailabilityDto>(availability);
        }


        public async Task<AvailabilityDto> SetAvailabilityAsync(AvailabilityDto availabilityDto)
        {
            try
            {
                await EnsureDoctorExistsAsync(availabilityDto.DoctorId);
                var availability = mapper.Map<Availability>(availabilityDto);
                var createdAvailability = await repository.AddAvailabilityAsync(availability);
                return mapper.Map<AvailabilityDto>(createdAvailability);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while setting new availability.");
                throw new ApplicationException("An error occurred while setting new availability.", ex);
            }
        }

        public async Task<AvailabilityDto> UpdateAvailabilityAsync(int doctorId, bool isAvailable)
        {
            try
            {
                await EnsureDoctorExistsAsync(doctorId);

                var availability = await repository.GetAvailabilityByDoctorIdAsync(doctorId);
                if (availability == null)
                {
                    logger.LogWarning($"Doctor with ID {doctorId} not found for update.");
                    throw new KeyNotFoundException($"Doctor with ID {doctorId} not found.");
                }

                availability.IsAvailable = isAvailable;
                availability.StartTime = isAvailable ? DateTime.Now : availability.StartTime;
                availability.EndTime = !isAvailable ? DateTime.Now : availability.EndTime;

                var updatedAvailability = await repository.UpdateAvailabilityAsync(availability);
                return mapper.Map<AvailabilityDto>(updatedAvailability);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred while updating availability for Doctor ID: {doctorId}");
                throw new ApplicationException("An error occurred while updating availability.", ex);
            }
        }
    }

}
