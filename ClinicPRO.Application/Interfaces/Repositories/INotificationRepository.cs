using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByAccountIdAsync(int accountId);
        Task<List<Notification>> GetPendingAsync(DateTime upToDateTime);
        Task<bool> ExistsAppointmentReminderAsync(int appointmentId);
        Task<bool> ExistsMedicationReminderForTodayAsync(int itemId, DateTime date);
        Task AddAsync(Notification notification);
        void Update(Notification notification);
    }
}