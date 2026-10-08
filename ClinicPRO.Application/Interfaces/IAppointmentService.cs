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
        Task<(bool Success, string Message)> AddSlotAsync(AddSlotDto dto);
        Task<List<DoctorSlotDto>> GetDoctorSlotsAsync(int doctorId);
        Task<(bool Success, string Message)> CancelAppointmentAsync(int appointmentId, int patientId);
        Task<(bool Success, string Message)> DeleteSlotAsync(int slotId, int doctorId);
    }
}