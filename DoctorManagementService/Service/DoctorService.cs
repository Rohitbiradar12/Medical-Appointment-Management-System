using AutoMapper;
using DoctorManagementService.Model.DTO;
using DoctorManagementService.Models;
using DoctorManagementService.Repository;

namespace DoctorManagementService.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(IDoctorRepository repository, IMapper mapper, ILogger<DoctorService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DoctorProfileDTO> GetDoctorProfileAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching doctor profile for ID: {DoctorId}", id);
                var doctor = await _repository.GetDoctorByIdAsync(id);
                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID: {DoctorId} not found.", id);
                    throw new KeyNotFoundException("Doctor not found.");
                }
                _logger.LogInformation("Successfully fetched doctor profile for ID: {DoctorId}", id);
                return _mapper.Map<DoctorProfileDTO>(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching doctor profile for ID: {DoctorId}", id);
                throw;
            }
        }

        public async Task UpdateDoctorProfileAsync(int id, DoctorProfileDTO profileDTO)
        {
            try
            {
                _logger.LogInformation("Updating doctor profile for ID: {DoctorId}", id);
                var doctor = await _repository.GetDoctorByIdAsync(id);
                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID: {DoctorId} not found.", id);
                    throw new KeyNotFoundException("Doctor not found.");
                }

                _mapper.Map(profileDTO, doctor);
                await _repository.UpdateDoctorAsync(doctor);
                _logger.LogInformation("Successfully updated doctor profile for ID: {DoctorId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating doctor profile for ID: {DoctorId}", id);
                throw;
            }
        }


        public async Task<IEnumerable<DoctorProfileDTO>> GetAllDoctorsAsync()
        {
            try
            {
                var doctors = await _repository.GetAllDoctorsAsync();
                return _mapper.Map<IEnumerable<DoctorProfileDTO>>(doctors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all doctors.");
                throw;
            }
        }

        public async Task<DoctorProfileDTO> AddDoctorAsync(DoctorProfileDTO profileDTO)
        {
            try
            {
                var doctor = _mapper.Map<Doctor>(profileDTO);
                var addedDoctor = await _repository.AddDoctorAsync(doctor);
                return _mapper.Map<DoctorProfileDTO>(addedDoctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new doctor.");
                throw;
            }
        }

        public async Task DeleteDoctorAsync(int id)
        {
            try
            {
                await _repository.DeleteDoctorAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor with ID: {Id}", id);
                throw;
            }
        }
    }
}
