using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<MedicalRecord?> GetByIdAsync(int recordId);
        Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId);
        Task<List<MedicalRecord>> GetByPatientIdAsync(int patientId);
        Task AddAsync(MedicalRecord record);
        void Update(MedicalRecord record);
        void Delete(MedicalRecord record);
    }
}