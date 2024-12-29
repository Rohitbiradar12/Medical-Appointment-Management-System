using DoctorManagementService.Context;
using DoctorManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorManagementService.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorContext _context;

        public DoctorRepository(DoctorContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            try
            {
                return await _context.Doctors.SingleOrDefaultAsync(d => d.Id == id)
                    ?? throw new KeyNotFoundException($"Doctor with ID {id} not found.");
            }
            catch (DbUpdateException ex)
            {
              
                throw new InvalidOperationException("Error fetching doctor details from the database.", ex);
            }
            catch (Exception ex)
            {
                
                throw new Exception("An unexpected error occurred while retrieving doctor details.", ex);
            }
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            try
            {
                return await _context.Doctors.ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving the doctors.");
            }
        }

        public async Task<Doctor> AddDoctorAsync(Doctor doctor)
        {
            try
            {
                await _context.Doctors.AddAsync(doctor);
                await SaveAsync();
                return doctor;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while adding the doctor.");
            }
        }

        public async Task DeleteDoctorAsync(int id)
        {
            try
            {
                var doctor = await GetDoctorByIdAsync(id);
                if (doctor == null)
                {
                    throw new KeyNotFoundException($"Doctor with ID: {id} not found.");
                }

                _context.Doctors.Remove(doctor);
                await SaveAsync();
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the doctor.");
            }
        }


        public async Task<Doctor> UpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                var doc =  await GetDoctorByIdAsync(doctor.Id);
                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync();  
                return doctor;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                
                throw new InvalidOperationException("The doctor record was modified by another process. Please try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                
                throw new InvalidOperationException("Error updating doctor details in the database.", ex);
            }
            catch (Exception ex)
            {
                
                throw new Exception("An unexpected error occurred while updating doctor details.", ex);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                
                throw new InvalidOperationException("Error saving changes to the database.", ex);
            }
            catch (Exception ex)
            {
                
                throw new Exception("An unexpected error occurred while saving changes.", ex);
            }
        }

        public async Task<bool> DoesDoctorExistAsync(int doctorId)
        {
            try
            {
                return await _context.Doctors.AnyAsync(d => d.Id == doctorId);
            }
            catch (DbUpdateException ex)
            {

                throw new InvalidOperationException("Error finding docotr in database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while finding doctor");
            }

        }
    }
}
