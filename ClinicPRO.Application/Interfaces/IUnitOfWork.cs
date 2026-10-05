using ClinicPRO.Application.Interfaces.Repositories;

namespace ClinicPRO.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IPatientRepository Patients { get; }
        IDoctorRepository Doctors { get; }
        ISpecializationRepository Specializations { get; }
        IAvailableSlotRepository AvailableSlots { get; }
        IAppointmentRepository Appointments { get; }
        IMedicalRecordRepository MedicalRecords { get; }
        IPrescriptionRepository Prescriptions { get; }
        IPrescriptionItemRepository PrescriptionItems { get; }
        IMedicineRepository Medicines { get; }
        IMedicationTimeRepository MedicationTimes { get; }
        INotificationRepository Notifications { get; }
        ISettingsRepository Settings { get; }

        Task<int> SaveChangesAsync();
    }
}