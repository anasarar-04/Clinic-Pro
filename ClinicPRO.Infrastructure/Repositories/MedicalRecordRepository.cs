using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecord?> GetByIdAsync(int recordId)
        {
            return await _context.MedicalRecords
                .Include(r => r.Doctor)
                .Include(r => r.Prescription)
                .FirstOrDefaultAsync(r => r.RecordId == recordId);
        }

        public async Task<List<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToListAsync();
        }

        public async Task AddAsync(MedicalRecord record)
        {
            await _context.MedicalRecords.AddAsync(record);
        }

        public void Update(MedicalRecord record)
        {
            _context.MedicalRecords.Update(record);
        }

        public void Delete(MedicalRecord record)
        {
            _context.MedicalRecords.Remove(record);
        }
        public async Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.MedicalRecords
                .Include(r => r.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(r => r.Doctor)
                    .ThenInclude(d => d.Specialization)
                .Include(r => r.Prescription)
                    .ThenInclude(p => p!.Items)
                        .ThenInclude(i => i.Medicine)
                .Include(r => r.Prescription)
                    .ThenInclude(p => p!.Items)
                        .ThenInclude(i => i.MedicationTimes)
                .FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);
        }
    }
}