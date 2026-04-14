using CSharpFunctionalExtensions;
using NotificationsService.Abstractions;
using NotificationsService.DTO;
using NotificationsService.Models;
using NotificationsService.NotifyStrategies;
using NotificationsService.Repositories;

namespace NotificationsService.Services
{
    public class NotificationsService : INotificationsService
    {
        HttpClientService httpClientService;
        IMassNotificationRepository notificationsRep;
        ILogger logger;
        public NotificationsService(HttpClientService clientsService,IMassNotificationRepository notificationRulesRepository,ILogger<NotificationsService> logger)
        {
            httpClientService= clientsService;
            notificationsRep= notificationRulesRepository;
            this.logger = logger;
        }
        public async Task NotifySubscribers(int userId)
        {
            User postman = httpClientService.RequestUser(userId);
            if (postman is null)
            {
                logger.LogError("пользователь указанный в сообщении не найден");
                return;
            }
            ICollection<Subscription> subsToBeNotified = httpClientService.RequestSubscribers(userId);
            ICollection<PersonalNotificationRule> rules=  await notificationsRep.Get(subsToBeNotified.Select(x => x.Id).ToArray());
            rules.ToList().AsParallel().ForAll((rule) =>
            {
                NotifySubscriber(postman, rule);
            });
        }
        private async Task NotifySubscriber(User postman,PersonalNotificationRule subscriberRule)
        {
            INotificationStrategy strategy=new DefaultNotificationStrategy();
            switch (subscriberRule.NotificationService)
            {
                case "mail.ru":
                    strategy = new EmailNotifyStrategy();
                    break;
            }
            strategy.Handle(subscriberRule.InternalServiceIdentificator, postman);
        }
    }
}
