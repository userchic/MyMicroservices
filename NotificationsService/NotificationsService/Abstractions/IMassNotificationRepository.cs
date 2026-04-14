using NotificationsService.Models;

namespace NotificationsService.Abstractions
{
    public interface IMassNotificationRepository:IRepository
    {
        Task<ICollection<PersonalNotificationRule>> Get(ICollection<int> userIds);
    }
}
