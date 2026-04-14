using NotificationsService.Models;

namespace NotificationsService.Abstractions
{
    public interface INotificationRulesRepository:IRepository
    {
        Task<PersonalNotificationRule?> Get(int userId);
        Task<ICollection<PersonalNotificationRule>> Get(ICollection<int> userId);
        Task Create(PersonalNotificationRule newRule);
        void Update(PersonalNotificationRule updatedRule);
        void Delete(PersonalNotificationRule rule);
    }
}
