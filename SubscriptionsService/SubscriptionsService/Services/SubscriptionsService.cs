using CSharpFunctionalExtensions;
using SubscriptionsService.Abstractions;
using SubscriptionsService.Models;

namespace SubscriptionsService.Services
{
    public class SubscriptionsService : ISubscriptionsService
    {
        ISubscriptionRepository _subsRep;
        public SubscriptionsService( ISubscriptionRepository subsRep)
        {
            _subsRep= subsRep;
        }
        public ICollection<Subscribtion> GetSubscribers(int userId)
        {
            return _subsRep.GetUserSubscribers(userId);
        }
        public ICollection<Subscribtion> GetSubscriptions(int userId)
        {
            return _subsRep.GetUserSubscriptions(userId);
        }
        public async Task<bool> IsSubscribed(int userId, int targetId)
        {
            return await _subsRep.GetIsSubscribed(userId, targetId);
        }
        public async Task<Result<string,string>> Subscribe(int userId, int targetId)
        {
            var currentSubs = _subsRep.GetUserSubscriptions(userId);
            if(currentSubs.FirstOrDefault((sub) => sub.SubscriberId == userId && sub.PosterId == targetId) is not null)
                return ((string)null).ToResult("Вы уже подписаны на этого пользователя");
            if (userId == targetId) 
                return ((string)null).ToResult("Нельзя подписаться на себя");
            await _subsRep.SubscribeUserOnUser(userId, targetId);
            await _subsRep.Save();
            return "Подписка успешна".ToResult("Здесь все должно быть нормально");
        }

        public async Task<Result<string,string>> UnSubscribe(int userId, int targetId)
        {
            var currentSubs = _subsRep.GetUserSubscriptions(userId);
            Subscribtion currentSubscription = currentSubs.FirstOrDefault((sub) => sub.SubscriberId == userId && sub.PosterId==targetId);
            if (currentSubscription is null)
                return ((string)null).ToResult("Вы не подписаны на этого пользователя");
            _subsRep.UnsubscribeUserFromUser(currentSubscription);
            await _subsRep.Save();
            return "Отписка успешна".ToResult("Здесь все должно быть нормально");
        }
    }
}
