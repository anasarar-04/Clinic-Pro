using System.ComponentModel.DataAnnotations;

namespace ClinicPRO.Domain.Entities
{
    public class Specialization
    {
        public int SpecializationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}