namespace ClinicPRO.Application.DTOs
{
    public class PrescriptionItemInputDto
    {
        public int MedicineId { get; set; }
        public string? Dosage { get; set; }
        public int DurationDays { get; set; }
        public string? Instructions { get; set; }
        public List<string> ReminderTimes { get; set; } = new();
    }
}