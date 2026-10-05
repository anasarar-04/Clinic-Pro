using ClinicPRO.Application.DTOs;

namespace ClinicPRO.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<DoctorListDto>> GetAllDoctorsAsync();
        Task<List<DoctorListDto>> GetDoctorsBySpecializationAsync(int specializationId);
        Task<List<AvailableSlotDto>> GetAvailableSlotsAsync(int doctorId);
        Task<(bool Success, string Message)> BookAppointmentAsync(BookAppointmentDto dto);
        Task<List<PatientAppointmentDto>> GetMyAppointmentsAsync(int patientId);
        Task<List<DoctorAppointmentDto>> GetDoctorAppointmentsAsync(int doctorId);
    }
}