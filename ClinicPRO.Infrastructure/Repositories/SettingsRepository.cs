using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Settings?> GetByAccountIdAsync(int accountId)
        {
            return await _context.Settings
                .FirstOrDefaultAsync(s => s.AccountId == accountId);
        }

        public async Task AddAsync(Settings settings)
        {
            await _context.Settings.AddAsync(settings);
        }

        public void Update(Settings settings)
        {
            _context.Settings.Update(settings);
        }
    }
}