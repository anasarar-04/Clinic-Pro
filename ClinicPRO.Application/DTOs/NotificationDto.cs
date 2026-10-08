namespace ClinicPRO.Application.DTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string NotifType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public bool IsSent { get; set; }
    }
}