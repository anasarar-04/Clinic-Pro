using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByIdAsync(int patientId)
        {
            return await _context.Patients
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        public async Task<Patient?> GetByAccountIdAsync(int accountId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.AccountId == accountId);
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }

        public void Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
        }
    }
}