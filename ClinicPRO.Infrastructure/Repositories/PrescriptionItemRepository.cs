using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class PrescriptionItemRepository : IPrescriptionItemRepository
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionItem?> GetByIdAsync(int itemId)
        {
            return await _context.PrescriptionItems
                .Include(i => i.Medicine)
                .FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        public async Task<List<PrescriptionItem>> GetByPrescriptionIdAsync(int prescriptionId)
        {
            return await _context.PrescriptionItems
                .Include(i => i.Medicine)
                .Where(i => i.PrescriptionId == prescriptionId)
                .ToListAsync();
        }

        public async Task AddAsync(PrescriptionItem item)
        {
            await _context.PrescriptionItems.AddAsync(item);
        }

        public void Update(PrescriptionItem item)
        {
            _context.PrescriptionItems.Update(item);
        }

        public void Delete(PrescriptionItem item)
        {
            _context.PrescriptionItems.Remove(item);
        }
        public async Task<List<PrescriptionItem>> GetActiveItemsAsync(DateTime today)
        {
            return await _context.PrescriptionItems
                .Include(i => i.Medicine)
                .Include(i => i.MedicationTimes)
                .Include(i => i.Prescription)
                    .ThenInclude(p => p.MedicalRecord)
                        .ThenInclude(r => r.Patient)
                            .ThenInclude(p => p.Account)
                .Where(i => i.Prescription.IssueDate.Date <= today.Date
                         && i.Prescription.IssueDate.Date.AddDays(i.DurationDays) >= today.Date)
                .ToListAsync();
        }
    }
}