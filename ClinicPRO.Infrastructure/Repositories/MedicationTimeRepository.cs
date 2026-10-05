using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class MedicationTimeRepository : IMedicationTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicationTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MedicationTime>> GetByItemIdAsync(int itemId)
        {
            return await _context.MedicationTimes
                .Where(mt => mt.ItemId == itemId)
                .ToListAsync();
        }

        public async Task AddAsync(MedicationTime medicationTime)
        {
            await _context.MedicationTimes.AddAsync(medicationTime);
        }

        public void Delete(MedicationTime medicationTime)
        {
            _context.MedicationTimes.Remove(medicationTime);
        }
    }
}