using ClinicPRO.Domain.Entities;

namespace ClinicPRO.Application.Interfaces.Repositories
{
    public interface ISpecializationRepository
    {
        Task<Specialization?> GetByIdAsync(int specializationId);
        Task<List<Specialization>> GetAllAsync();
        Task AddAsync(Specialization specialization);
        void Update(Specialization specialization);
        void Delete(Specialization specialization);
    }
}