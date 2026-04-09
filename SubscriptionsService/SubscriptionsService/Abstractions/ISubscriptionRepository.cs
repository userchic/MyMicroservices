using SubscriptionsService.Models;
using Microsoft.EntityFrameworkCore;

namespace SubscriptionsService.Abstractions
{
    public interface ISubscriptionRepository : IRepository
    {
        Task<bool> GetIsSubscribed(int userId, int targetUserId);
        ICollection<Subscribtion> GetUserSubscriptions(int userId);
        ICollection<Subscribtion> GetUserSubscribers(int userId);
        Task SubscribeUserOnUser(int userId, int newSubscriberId);
        void UnsubscribeUserFromUser(Subscribtion subscription);
    }
}
