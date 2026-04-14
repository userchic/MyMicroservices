using MailKit.Net.Smtp;
using MimeKit;
using NotificationsService.Abstractions;
using NotificationsService.DTO;


namespace NotificationsService.NotifyStrategies
{
    public class EmailNotifyStrategy : INotificationStrategy
    {
        public void Handle(string email,User postmanInfo)
        {
            sendEmailNotificationMessage(email, postmanInfo);
        }
        private void sendEmailNotificationMessage(string email, User postmanInfo)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MathBattlesSender", "Math.Battles@mail.ru"));
            message.To.Add(new MailboxAddress("TargetTeamMember", email));
            message.Subject = "Уведомление о публикации";
            var builder = new BodyBuilder();
            builder.TextBody = $"Пользователь {postmanInfo.Name} {postmanInfo.Surname} {postmanInfo.Fatname} - {postmanInfo.Login} опубликовал пост $ссылка на страницу$ ";
            message.Body = builder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.mail.ru", 25, false);
                client.Authenticate("Math.Battles@mail.ru", "J5aJOwNVJ4I6qGaQYnXP");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
