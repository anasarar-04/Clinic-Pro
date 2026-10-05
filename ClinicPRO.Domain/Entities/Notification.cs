using System.ComponentModel.DataAnnotations;
using ClinicPRO.Domain.Enums;

namespace ClinicPRO.Domain.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public int? ItemId { get; set; }
        public PrescriptionItem? PrescriptionItem { get; set; }

        public NotificationType NotifType { get; set; }

        [MaxLength(300)]
        public string Message { get; set; }

        public DateTime ScheduledAt { get; set; }
        public bool IsSent { get; set; } = false;
    }
}