using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IAvailableSlotRepository
    {
        Task<AvailableSlot?> GetByIdAsync(int slotId);
        Task<List<AvailableSlot>> GetAvailableByDoctorIdAsync(int doctorId);
        Task AddAsync(AvailableSlot slot);
        void Update(AvailableSlot slot);
        void Delete(AvailableSlot slot);
    }
}