using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IMedicineRepository
    {
        Task<Medicine?> GetByIdAsync(int medicineId);
        Task<List<Medicine>> SearchByNameAsync(string term);
        Task<List<Medicine>> GetAllAsync();
        Task AddAsync(Medicine medicine);
        void Update(Medicine medicine);
        void Delete(Medicine medicine);
    }
}