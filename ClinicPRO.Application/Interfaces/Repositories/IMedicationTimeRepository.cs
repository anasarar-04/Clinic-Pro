using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IMedicationTimeRepository
    {
        Task<List<MedicationTime>> GetByItemIdAsync(int itemId);
        Task AddAsync(MedicationTime medicationTime);
        void Delete(MedicationTime medicationTime);
    }
}