using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<Prescription?> GetByIdAsync(int prescriptionId);
        Task<Prescription?> GetByRecordIdAsync(int recordId);
        Task AddAsync(Prescription prescription);
        void Update(Prescription prescription);
        void Delete(Prescription prescription);
    }
}