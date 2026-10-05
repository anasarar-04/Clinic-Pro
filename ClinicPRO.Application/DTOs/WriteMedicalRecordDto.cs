namespace ClinicPRO.Application.DTOs
{
    public class WriteMedicalRecordDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string? Diagnosis { get; set; }
        public string? Report { get; set; }
        public List<PrescriptionItemInputDto> Items { get; set; } = new();
    }
}