namespace ClinicPRO.Domain.Enums
{
    public enum UserRole
    {
        Patient,
        Doctor
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public enum NotificationType
    {
        AppointmentReminder,
        MedicationReminder
    }
}