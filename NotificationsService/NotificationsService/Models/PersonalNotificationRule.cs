using NotificationsService.DTO;
using System.ComponentModel.DataAnnotations;

namespace NotificationsService.Models
{
    public class PersonalNotificationRule
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NotificationService { get; set; }
        public string InternalServiceIdentificator { get; set; }
        public static PersonalNotificationRule Create(CreateNotificationRulesRequest request)
        {
            return new PersonalNotificationRule()
            {
                InternalServiceIdentificator = request.InternalServiceIdentificator,
                NotificationService = request.NotificationService,
            };
        }
        public static PersonalNotificationRule Create(UpdateNotificationRulesRequest request)
        {
            return new PersonalNotificationRule()
            {
                InternalServiceIdentificator = request.InternalServiceIdentificator,
                NotificationService = request.NotificationService,
            };
        }
    }
}
