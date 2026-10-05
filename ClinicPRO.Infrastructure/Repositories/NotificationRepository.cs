using ClinicPRO.Application.Interfaces.Repositories;
using ClinicPRO.Domain.Entities;
using ClinicPRO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicPRO.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetByAccountIdAsync(int accountId)
        {
            return await _context.Notifications
                .Where(n => n.AccountId == accountId)
                .OrderByDescending(n => n.ScheduledAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetPendingAsync(DateTime upToDateTime)
        {
            return await _context.Notifications
                .Where(n => !n.IsSent && n.ScheduledAt <= upToDateTime)
                .ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public void Update(Notification notification)
        {
            _context.Notifications.Update(notification);
        }
    }
}