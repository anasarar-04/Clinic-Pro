using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int accountId);
        Task<Account?> GetByNationalIdAsync(string nationalId);
        Task<List<Account>> GetAllAsync();
        Task AddAsync(Account account);
        void Update(Account account);
        void Delete(Account account);
    }
}