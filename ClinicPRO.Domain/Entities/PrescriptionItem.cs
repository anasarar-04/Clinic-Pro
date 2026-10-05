using System.ComponentModel.DataAnnotations;

namespace ClinicPRO.Domain.Entities
{
    public class PrescriptionItem
    {
        [Key]
        public int ItemId { get; set; }

        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        [MaxLength(50)]
        public string? Dosage { get; set; }

        public int DurationDays { get; set; }

        [MaxLength(300)]
        public string? Instructions { get; set; }

        public ICollection<MedicationTime> MedicationTimes { get; set; } = new List<MedicationTime>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}