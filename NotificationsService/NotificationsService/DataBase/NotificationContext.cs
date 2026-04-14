using Microsoft.EntityFrameworkCore;
using NotificationsService.Models;

namespace NotificationsService.DataBase
{
    public class NotificationContext:DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
            bool isCreated = Database.EnsureCreated();
        }
        public DbSet<PersonalNotificationRule> Rules { get; set; }
    }
}
