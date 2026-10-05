using ClinicPRO.Domain.Enums;

namespace ClinicPRO.Application.DTOs
{
    public class PatientAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string SpecializationName { get; set; } = string.Empty;
        public DateTime? SlotDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}