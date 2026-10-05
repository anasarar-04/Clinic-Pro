using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByIdAsync(int doctorId);
        Task<Doctor?> GetByAccountIdAsync(int accountId);
        Task<List<Doctor>> GetBySpecializationIdAsync(int specializationId);
        Task<List<Doctor>> GetAllAsync();
        Task AddAsync(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(Doctor doctor);
    }
}