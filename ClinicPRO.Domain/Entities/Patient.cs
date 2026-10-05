namespace ClinicPRO.Domain.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}