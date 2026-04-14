using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotificationsService.Abstractions;
using NotificationsService.DataBase;
using NotificationsService.Models;

namespace NotificationsService.Repositories
{
    public class MassNotificationRepository : IMassNotificationRepository
    {
        NotificationContext _context;
        public MassNotificationRepository(IConfiguration config)
        {
            DbContextOptionsBuilder<NotificationContext> builder = new DbContextOptionsBuilder<NotificationContext>().UseNpgsql(config.GetConnectionString("DefaultConnection"));
            _context = new NotificationContext(builder.Options);
        }
        public async Task<ICollection<PersonalNotificationRule>> Get(ICollection<int> userId)
        {
            return await _context.Rules.Where((rule) => userId.Contains(rule.UserId)).ToArrayAsync();
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
