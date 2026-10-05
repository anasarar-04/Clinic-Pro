using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetByIdAsync(int doctorId)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }

        public async Task<Doctor?> GetByAccountIdAsync(int accountId)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.AccountId == accountId);
        }

        public async Task<List<Doctor>> GetBySpecializationIdAsync(int specializationId)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Where(d => d.SpecializationId == specializationId)
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Include(d => d.Specialization)
                .ToListAsync();
        }

        public async Task AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
        }

        public void Update(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
        }
    }
}