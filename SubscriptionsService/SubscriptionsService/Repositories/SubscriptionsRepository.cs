using SubscriptionsService.Abstractions;
using SubscriptionsService.DataBase;
using SubscriptionsService.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace SubscriptionsService.Repositories
{
    public class SubscriptionsRepository : ISubscriptionRepository
    {
        public SubscriptionsContext _context;
        public SubscriptionsRepository(SubscriptionsContext context)
        {
            _context = context;
        }

        public ICollection<Subscribtion> GetUserSubscriptions(int userId)
        {
            return _context.Subscriptions.Where((subscription) => subscription.SubscriberId == userId).ToArray();
        }
        public ICollection<Subscribtion> GetUserSubscribers(int userId)
        {
            return _context.Subscriptions.Where((subscription) => subscription.PosterId == userId).ToArray();
        }
        public async Task SubscribeUserOnUser(int userId, int newSubscriberId)
        {
            await _context.Subscriptions.AddAsync(new Subscribtion() { SubscriberId= userId ,PosterId=newSubscriberId});
        }
        public void UnsubscribeUserFromUser(Subscribtion subscription)
        {
            _context.Subscriptions.Remove(subscription);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> GetIsSubscribed(int userId, int targetUserId)
        {
            if (await _context.Subscriptions.FirstOrDefaultAsync((subscription) => subscription.SubscriberId == userId && subscription.PosterId == targetUserId) is not null)
                return true;
            return false;
        }
    }
}
