using ClinicPRO.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<AvailableSlot> AvailableSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicationTime> MedicationTimes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---------- Account: NationalId لازم يكون فريد (Unique) ----------
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.NationalId)
                .IsUnique();

            // ---------- Account <-> Patient (1:1) ----------
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Account)
                .WithOne(a => a.Patient)
                .HasForeignKey<Patient>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Account <-> Doctor (1:1) ----------
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Account)
                .WithOne(a => a.Doctor)
                .HasForeignKey<Doctor>(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Account <-> Settings (1:1) ----------
            modelBuilder.Entity<Settings>()
                .HasOne(s => s.Account)
                .WithOne(a => a.Settings)
                .HasForeignKey<Settings>(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Doctor -> Specialization (N:1) ----------
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- AvailableSlot -> Doctor (N:1) ----------
            modelBuilder.Entity<AvailableSlot>()
                .HasOne(sl => sl.Doctor)
                .WithMany(d => d.AvailableSlots)
                .HasForeignKey(sl => sl.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Appointment -> Patient / Doctor / Slot ----------
            modelBuilder.Entity<Appointment>()
                .HasOne(ap => ap.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(ap => ap.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(ap => ap.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(ap => ap.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment <-> AvailableSlot : 0,1 <-> 1
            modelBuilder.Entity<Appointment>()
                .HasOne(ap => ap.Slot)
                .WithOne(sl => sl.Appointment)
                .HasForeignKey<Appointment>(ap => ap.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- MedicalRecord -> Patient / Doctor / Appointment ----------
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(mr => mr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Appointment)
                .WithOne(ap => ap.MedicalRecord)
                .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Prescription -> MedicalRecord (1:0,1) ----------
            modelBuilder.Entity<Prescription>()
                .HasOne(pr => pr.MedicalRecord)
                .WithOne(mr => mr.Prescription)
                .HasForeignKey<Prescription>(pr => pr.RecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- PrescriptionItem -> Prescription / Medicine ----------
            modelBuilder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Prescription)
                .WithMany(pr => pr.Items)
                .HasForeignKey(pi => pi.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Medicine)
                .WithMany(m => m.PrescriptionItems)
                .HasForeignKey(pi => pi.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- MedicationTime -> PrescriptionItem ----------
            modelBuilder.Entity<MedicationTime>()
                .HasOne(mt => mt.PrescriptionItem)
                .WithMany(pi => pi.MedicationTimes)
                .HasForeignKey(mt => mt.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Notification -> Account / Appointment / PrescriptionItem ----------
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Account)
                .WithMany(a => a.Notifications)
                .HasForeignKey(n => n.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Appointment)
                .WithMany(ap => ap.Notifications)
                .HasForeignKey(n => n.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.PrescriptionItem)
                .WithMany(pi => pi.Notifications)
                .HasForeignKey(n => n.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}