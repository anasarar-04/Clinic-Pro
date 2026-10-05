namespace ClinicPRO.Domain.Entities
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        public int RecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.Now;

        public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
    }
}