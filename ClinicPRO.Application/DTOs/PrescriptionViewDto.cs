namespace ClinicPRO.Application.DTOs
{
    public class PrescriptionViewDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public string SpecializationName { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public string? Diagnosis { get; set; }
        public string? Report { get; set; }
        public List<PrescriptionItemViewDto> Items { get; set; } = new();
    }

    public class PrescriptionItemViewDto
    {
        public string MedicineName { get; set; } = string.Empty;
        public string? Dosage { get; set; }
        public int DurationDays { get; set; }
        public string? Instructions { get; set; }
        public List<string> ReminderTimes { get; set; } = new();
    }
}