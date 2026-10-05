namespace ClinicPRO.Domain.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }

        public string? Bio { get; set; }

        public ICollection<AvailableSlot> AvailableSlots { get; set; } = new List<AvailableSlot>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}