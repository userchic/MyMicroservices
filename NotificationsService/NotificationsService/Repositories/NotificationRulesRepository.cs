using Microsoft.EntityFrameworkCore;
using NotificationsService.Abstractions;
using NotificationsService.DataBase;
using NotificationsService.Models;

namespace NotificationsService.Repositories
{
    public class NotificationRulesRepository : INotificationRulesRepository
    {
        NotificationContext _context;
        public NotificationRulesRepository(NotificationContext context)
        {
            _context = context;
        }
        public async Task<PersonalNotificationRule?> Get(int userId)
        {
            return await _context.Rules.FirstOrDefaultAsync((rule) => rule.UserId == userId);
        }

        public async Task<ICollection<PersonalNotificationRule>> Get(ICollection<int> userId)
        {
            return await _context.Rules.Where((rule) => userId.Contains(rule.UserId)).ToArrayAsync();
        }
        public async Task Create(PersonalNotificationRule newRule)
        {
            await _context.Rules.AddAsync(newRule);
        }
        public void Update(PersonalNotificationRule updatedRule)
        {
            _context.Rules.Update(updatedRule);
        }
        public void Delete(PersonalNotificationRule rule)
        {
            _context.Remove(rule);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
