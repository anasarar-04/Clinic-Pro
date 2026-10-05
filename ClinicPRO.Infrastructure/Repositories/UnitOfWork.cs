using ClinicPRO.Application.Interfaces;
using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Infrastructure.Data;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IAccountRepository Accounts { get; }
        public IPatientRepository Patients { get; }
        public IDoctorRepository Doctors { get; }
        public ISpecializationRepository Specializations { get; }
        public IAvailableSlotRepository AvailableSlots { get; }
        public IAppointmentRepository Appointments { get; }
        public IMedicalRecordRepository MedicalRecords { get; }
        public IPrescriptionRepository Prescriptions { get; }
        public IPrescriptionItemRepository PrescriptionItems { get; }
        public IMedicineRepository Medicines { get; }
        public IMedicationTimeRepository MedicationTimes { get; }
        public INotificationRepository Notifications { get; }
        public ISettingsRepository Settings { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IAccountRepository accounts,
            IPatientRepository patients,
            IDoctorRepository doctors,
            ISpecializationRepository specializations,
            IAvailableSlotRepository availableSlots,
            IAppointmentRepository appointments,
            IMedicalRecordRepository medicalRecords,
            IPrescriptionRepository prescriptions,
            IPrescriptionItemRepository prescriptionItems,
            IMedicineRepository medicines,
            IMedicationTimeRepository medicationTimes,
            INotificationRepository notifications,
            ISettingsRepository settings)
        {
            _context = context;
            Accounts = accounts;
            Patients = patients;
            Doctors = doctors;
            Specializations = specializations;
            AvailableSlots = availableSlots;
            Appointments = appointments;
            MedicalRecords = medicalRecords;
            Prescriptions = prescriptions;
            PrescriptionItems = prescriptionItems;
            Medicines = medicines;
            MedicationTimes = medicationTimes;
            Notifications = notifications;
            Settings = settings;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}