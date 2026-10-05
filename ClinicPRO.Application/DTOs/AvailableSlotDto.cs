namespace ClinicPRO.Application.DTOs
{
    public class AvailableSlotDto
    {
        public int SlotId { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}