using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(int patientId);
        Task<Patient?> GetByAccountIdAsync(int accountId);
        Task AddAsync(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
    }
}