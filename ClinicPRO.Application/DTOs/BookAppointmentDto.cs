namespace ClinicPRO.Application.DTOs
{
    public class BookAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SlotId { get; set; }
    }
}