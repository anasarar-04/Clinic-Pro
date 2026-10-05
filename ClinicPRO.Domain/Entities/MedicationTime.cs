using System.ComponentModel.DataAnnotations;

namespace ClinicPRO.Domain.Entities
{
    public class MedicationTime
    {
        [Key]
        public int TimeId { get; set; }

        public int ItemId { get; set; }
        public PrescriptionItem PrescriptionItem { get; set; }

        public TimeSpan ReminderTime { get; set; }
    }
}