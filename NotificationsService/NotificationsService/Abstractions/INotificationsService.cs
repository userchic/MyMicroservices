using CSharpFunctionalExtensions;

namespace NotificationsService.Abstractions
{
    public interface INotificationsService
    {
        Task NotifySubscribers(int userId);
    }
}
