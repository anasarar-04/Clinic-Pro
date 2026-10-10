using ClinicPRO.Application.DTOs;

namespace Clinic_PRO.Models
{
    public class PatientDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public int UpcomingCount { get; set; }
        public int CompletedCount { get; set; }
        public int NotificationsCount { get; set; }
        public PatientAppointmentDto? NextAppointment { get; set; }
        public List<PatientAppointmentDto> RecentAppointments { get; set; } = new();
        public List<NotificationDto> LatestNotifications { get; set; } = new();
    }

    public class DoctorDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public int TodayCount { get; set; }
        public int PendingCount { get; set; }
        public int CompletedCount { get; set; }
        public int FreeSlotsCount { get; set; }
        public List<DoctorAppointmentDto> TodayAppointments { get; set; } = new();
        public List<DoctorAppointmentDto> UpcomingAppointments { get; set; } = new();
    }
}