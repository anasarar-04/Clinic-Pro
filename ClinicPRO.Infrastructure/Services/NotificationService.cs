using ClinicPRO.Application.DTOs;
using ClinicPRO.Application.Interfaces;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Domain.Enums;

namespace ClinicPRO.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<NotificationDto>> GetMyNotificationsAsync(int accountId)
        {
            var notifications = await _unitOfWork.Notifications.GetByAccountIdAsync(accountId);

            return notifications.Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                NotifType = n.NotifType.ToString(),
                Message = n.Message,
                ScheduledAt = n.ScheduledAt,
                IsSent = n.IsSent
            }).ToList();
        }

        public async Task GenerateAppointmentRemindersAsync()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var appointments = await _unitOfWork.Appointments.GetBySlotDateAsync(tomorrow);

            foreach (var appt in appointments)
            {
                bool exists = await _unitOfWork.Notifications.ExistsAppointmentReminderAsync(appt.AppointmentId);
                if (exists) continue;

                var notification = new Notification
                {
                    AccountId = appt.Patient.AccountId,
                    AppointmentId = appt.AppointmentId,
                    NotifType = NotificationType.AppointmentReminder,
                    Message = $"تذكير: لديك موعد غدا الساعة {appt.Slot!.StartTime:hh\\:mm}",
                    ScheduledAt = DateTime.Now,
                    IsSent = true
                };

                await _unitOfWork.Notifications.AddAsync(notification);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task GenerateMedicationRemindersAsync()
        {
            var today = DateTime.Today;
            var now = DateTime.Now.TimeOfDay;
            var items = await _unitOfWork.PrescriptionItems.GetActiveItemsAsync(today);

            foreach (var item in items)
            {
                foreach (var time in item.MedicationTimes)
                {
                    // هامش 5 دقائق حتى ما نفوت الوقت المضبوط بالظبط
                    if (Math.Abs((now - time.ReminderTime).TotalMinutes) > 5)
                        continue;

                    bool exists = await _unitOfWork.Notifications.ExistsMedicationReminderForTodayAsync(item.ItemId, today);
                    if (exists) continue;

                    var accountId = item.Prescription.MedicalRecord.Patient.AccountId;

                    var notification = new Notification
                    {
                        AccountId = accountId,
                        ItemId = item.ItemId,
                        NotifType = NotificationType.MedicationReminder,
                        Message = $"تذكير: حان وقت أخذ دواء {item.Medicine.Name}",
                        ScheduledAt = DateTime.Now,
                        IsSent = true
                    };

                    await _unitOfWork.Notifications.AddAsync(notification);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}