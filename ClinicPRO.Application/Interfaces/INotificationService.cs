using ClinicPRO.Application.DTOs;

namespace ClinicPRO.Application.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetMyNotificationsAsync(int accountId);
        Task GenerateAppointmentRemindersAsync();
        Task GenerateMedicationRemindersAsync();
    }
}