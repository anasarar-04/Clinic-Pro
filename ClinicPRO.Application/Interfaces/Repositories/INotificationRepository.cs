using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByAccountIdAsync(int accountId);
        Task<List<Notification>> GetPendingAsync(DateTime upToDateTime);
        Task AddAsync(Notification notification);
        void Update(Notification notification);
    }
}