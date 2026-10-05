using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IPrescriptionItemRepository
    {
        Task<PrescriptionItem?> GetByIdAsync(int itemId);
        Task<List<PrescriptionItem>> GetByPrescriptionIdAsync(int prescriptionId);
        Task AddAsync(PrescriptionItem item);
        void Update(PrescriptionItem item);
        void Delete(PrescriptionItem item);
    }
}