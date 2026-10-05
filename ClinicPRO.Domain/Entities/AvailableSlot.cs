using System.ComponentModel.DataAnnotations;

namespace ClinicPRO.Domain.Entities
{
    public class AvailableSlot
    {
        [Key]
        public int SlotId { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; } = false;

        public Appointment? Appointment { get; set; }   // العلاقة صارت 1 <-> 0,1
    }
}