using CSharpFunctionalExtensions;
using NotificationsService.Abstractions;
using NotificationsService.DTO;
using NotificationsService.Models;

namespace NotificationsService.Services
{
    public class PersonalNotificationRulesService : IPersonalNotificationRulesService
    {
        INotificationRulesRepository _notifRep; 
        public PersonalNotificationRulesService(INotificationRulesRepository notifRep)
        {
            _notifRep= notifRep;
        }
        public async Task<PersonalNotificationRule?> GetPersonalRule(int userId)
        {
            return await _notifRep.Get(userId);
        }
        public async Task<Result<string, string>> CreatePersonalRule(CreateNotificationRulesRequest request, int userId)
        {
            var currentRule = await _notifRep.Get(userId);
            if(currentRule is not null)
                return ((string)null).ToResult("Yже существует правило уведомления вас.");
            PersonalNotificationRule newRule=  PersonalNotificationRule.Create(request);
            await _notifRep.Create(newRule);
            newRule.UserId = userId;
            await _notifRep.Save();
            return "Успешно создано правило".ToResult("Тут все должно быть нормально");
        }
        public async Task<Result<string, string>> UpdatePersonalRule(UpdateNotificationRulesRequest request, int userId)
        {
            var currentRule = await _notifRep.Get(userId);
            if (currentRule is null)
                return ((string)null).ToResult("Еще не создано правило уведомления вас, сначала его нужно создать.");
            PersonalNotificationRule newRule = PersonalNotificationRule.Create(request);
                  _notifRep.Update(newRule);
            await _notifRep.Save();
            return "Успешно отредактировано правило".ToResult("Тут все должно быть нормально");
        }
        public async Task<Result<string, string>> DeletePersonalRule(int userId)
        {
            var currentRule = await _notifRep.Get(userId);
            if (currentRule is null)
                return ((string)null).ToResult("Вы еще не создали правило уведомления себя чтобы удалять его.");
                  _notifRep.Delete(currentRule);
            await _notifRep.Save();
            return "Успешно удалено правило".ToResult("Тут все должно быть нормально");
        }

        
    }
}
