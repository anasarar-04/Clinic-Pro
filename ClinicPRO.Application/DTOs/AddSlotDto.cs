namespace ClinicPRO.Application.DTOs
{
    public class AddSlotDto
    {
        public int DoctorId { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}