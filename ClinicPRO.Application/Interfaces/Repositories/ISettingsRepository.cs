using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface ISettingsRepository
    {
        Task<Settings?> GetByAccountIdAsync(int accountId);
        Task AddAsync(Settings settings);
        void Update(Settings settings);
    }
}