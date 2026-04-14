using NotificationsService.DTO;

namespace NotificationsService.Abstractions
{
    public interface INotificationStrategy
    {
        public void Handle(string serviceUsername,User postmanInfo );
    }
}
