using Microsoft.EntityFrameworkCore;
using SubscriptionsService.Models;

namespace SubscriptionsService.DataBase
{
    public class SubscriptionsContext:DbContext
    {
        public SubscriptionsContext(DbContextOptions<SubscriptionsContext> options): base(options)
        {
            //Database.EnsureDeleted();
            bool isCreated = Database.EnsureCreated();
        }
        public DbSet<Subscribtion> Subscriptions { get; set; }
    }
}
