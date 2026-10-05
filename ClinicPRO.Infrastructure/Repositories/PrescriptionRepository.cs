using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Prescription?> GetByIdAsync(int prescriptionId)
        {
            return await _context.Prescriptions
                .Include(p => p.Items)
                    .ThenInclude(i => i.Medicine)
                .FirstOrDefaultAsync(p => p.PrescriptionId == prescriptionId);
        }

        public async Task<Prescription?> GetByRecordIdAsync(int recordId)
        {
            return await _context.Prescriptions
                .Include(p => p.Items)
                    .ThenInclude(i => i.Medicine)
                .FirstOrDefaultAsync(p => p.RecordId == recordId);
        }

        public async Task AddAsync(Prescription prescription)
        {
            await _context.Prescriptions.AddAsync(prescription);
        }

        public void Update(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
        }

        public void Delete(Prescription prescription)
        {
            _context.Prescriptions.Remove(prescription);
        }
    }
}