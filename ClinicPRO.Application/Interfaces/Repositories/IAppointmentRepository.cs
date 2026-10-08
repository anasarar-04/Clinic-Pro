using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int appointmentId);
        Task<List<Appointment>> GetByPatientIdAsync(int patientId);
        Task<List<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task<List<Appointment>> GetUpcomingByPatientIdAsync(int patientId, DateTime fromDate);
        Task<List<Appointment>> GetBySlotDateAsync(DateTime date);
        Task<Appointment?> GetBySlotIdAsync(int slotId);
        Task AddAsync(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Appointment appointment);
    }
}