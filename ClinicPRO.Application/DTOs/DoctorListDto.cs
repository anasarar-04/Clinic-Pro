namespace ClinicPRO.Application.DTOs
{
    public class DoctorListDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SpecializationName { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }
}