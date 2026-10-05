using System.ComponentModel.DataAnnotations;


namespace ClinicPRO.Domain.Entities
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public DateTime VisitDate { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Diagnosis { get; set; }

        public string? Report { get; set; }

        public Prescription? Prescription { get; set; }
    }
}