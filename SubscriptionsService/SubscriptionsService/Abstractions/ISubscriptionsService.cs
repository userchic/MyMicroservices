using CSharpFunctionalExtensions;
using SubscriptionsService.Models;

namespace SubscriptionsService.Abstractions
{
    public interface ISubscriptionsService
    {
        Task<bool> IsSubscribed(int userId, int targetId);
        ICollection<Subscribtion> GetSubscriptions(int userId);
        ICollection<Subscribtion> GetSubscribers(int userId);
        Task<Result<string, string>> Subscribe(int userId, int targetId);
        Task<Result<string, string>> UnSubscribe(int userId, int targetId);
    }
}
