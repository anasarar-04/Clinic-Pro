using ClinicPRO.Domain.Enums;

namespace ClinicPRO.Domain.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int? SlotId { get; set; }         // nullable لأنها صارت 0,1
        public AvailableSlot? Slot { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public MedicalRecord? MedicalRecord { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}