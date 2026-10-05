using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class AvailableSlotRepository : IAvailableSlotRepository
    {
        private readonly ApplicationDbContext _context;

        public AvailableSlotRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AvailableSlot?> GetByIdAsync(int slotId)
        {
            return await _context.AvailableSlots.FindAsync(slotId);
        }

        public async Task<List<AvailableSlot>> GetAvailableByDoctorIdAsync(int doctorId)
        {
            return await _context.AvailableSlots
                .Where(s => s.DoctorId == doctorId && !s.IsBooked)
                .ToListAsync();
        }

        public async Task AddAsync(AvailableSlot slot)
        {
            await _context.AvailableSlots.AddAsync(slot);
        }

        public void Update(AvailableSlot slot)
        {
            _context.AvailableSlots.Update(slot);
        }

        public void Delete(AvailableSlot slot)
        {
            _context.AvailableSlots.Remove(slot);
        }
    }
}