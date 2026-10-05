using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecializationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Specialization?> GetByIdAsync(int specializationId)
        {
            return await _context.Specializations.FindAsync(specializationId);
        }

        public async Task<List<Specialization>> GetAllAsync()
        {
            return await _context.Specializations.ToListAsync();
        }

        public async Task AddAsync(Specialization specialization)
        {
            await _context.Specializations.AddAsync(specialization);
        }

        public void Update(Specialization specialization)
        {
            _context.Specializations.Update(specialization);
        }

        public void Delete(Specialization specialization)
        {
            _context.Specializations.Remove(specialization);
        }
    }
}