using System.ComponentModel.DataAnnotations;

namespace ClinicPRO.Domain.Entities
{
    public class Medicine
    {
        public int MedicineId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string? GenericName { get; set; }

        [MaxLength(50)]
        public string? Form { get; set; }        // أقراص، شراب، حقن...

        [MaxLength(80)]
        public string? Category { get; set; }

        public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
    }
}