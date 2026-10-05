using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Medicine?> GetByIdAsync(int medicineId)
        {
            return await _context.Medicines.FindAsync(medicineId);
        }

        public async Task<List<Medicine>> SearchByNameAsync(string term)
        {
            return await _context.Medicines
                .Where(m => m.Name.Contains(term))
                .OrderBy(m => m.Name)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<Medicine>> GetAllAsync()
        {
            return await _context.Medicines.ToListAsync();
        }

        public async Task AddAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
        }

        public void Update(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
        }

        public void Delete(Medicine medicine)
        {
            _context.Medicines.Remove(medicine);
        }
    }
}