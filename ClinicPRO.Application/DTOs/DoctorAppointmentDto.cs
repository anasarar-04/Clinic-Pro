using ClinicPRO.Domain.Enums;

namespace ClinicPRO.Application.DTOs
{
    public class DoctorAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime? SlotDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}