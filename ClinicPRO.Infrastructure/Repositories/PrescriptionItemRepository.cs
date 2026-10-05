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
    }
}