namespace ClinicPRO.Application.DTOs
{
    public class DoctorSlotDto
    {
        public int SlotId { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
    }
}