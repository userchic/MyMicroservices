using CSharpFunctionalExtensions;
using MessageService.Abstractions;
using MessageService.DTO;
using MessageService.Models;
using MessageService.Repositories;

namespace MessageService.Services
{
    public class MessageService : IMessageService
    {
        IMessageRepository messageRep;
        IDialogRepository dialogRep;
        ILogger? logger;
        public MessageService(IMessageRepository messageRep, IDialogRepository dialogRep,ILogger<MessageService>? logger)
        {
            this.messageRep= messageRep;
            this.dialogRep= dialogRep;
            this.logger = logger;
        }
        public Dialog? GetDialog(int dialogId)
        {
            var dialog = dialogRep.GetDialog(dialogId);
            return dialog;
        }
        public Dialog? GetDialog(int targetUserId,int userId)
        {
            var dialog = dialogRep.GetDialog(targetUserId,userId);
            return dialog;
        }
        public List<Dialog> GetDialogsPage( int userId, int page)
        {
            return dialogRep.GetUserDialogues(userId,page);
        }
        public List<Message> GetMessagesPageFromDialog(int dialogId, int userId, int page)
        {
            var targetDialog = dialogRep.GetDialog(dialogId);
            if (targetDialog is null)
            {
                logger?.LogWarning("Получен запрос на получение страницы сообщений не существующего диалога пользователя ID:{userId} с пользователем ID:{ID}", userId,dialogId);
                return new List<Message>();
            }
            if (targetDialog.User1Id == userId || targetDialog.User2Id == userId) 
            { 
                return messageRep.GetMessagesPageFromDialog(dialogId, page); 
            }
            logger?.LogWarning("Получен запрос на получение страницы сообщений диалога ID:{ID} к которому пользователь ID:{userId} не имеет отношения.",targetDialog.Id, dialogId);
            return new List<Message>();
        }
        public async Task<Message> CreateMessage(CreateMessageRequest request, int userId)
        {
            var targetDialog = dialogRep.GetDialog(userId, request.RecieverId);
            if (targetDialog is null)
            {
                targetDialog=new Dialog() { User1Id = userId ,User2Id=request.RecieverId,Messages=new List<Message>()};
                targetDialog = await dialogRep.CreateDialog(targetDialog);
                await dialogRep.Save();
            }
            Message newMessage = new Message()
            {
                DialogId=targetDialog.Id,
                SenderId=userId,
                Text=request.Text,
                CreationTime=DateTime.UtcNow
            };
            newMessage = await messageRep.CreateMessage(newMessage);
            await messageRep.Save();
            return newMessage;
        }
        public async Task<Result<string, string>> UpdateMessage(UpdateMessageRequest request, int userId)
        {
            var targetMessage = messageRep.GetMessage(request.MessageId);
            if (targetMessage.SenderId!=userId)
            {
                logger?.LogWarning("Получен запрос на редактирование сообщения ID:{messageId} которым пользователь ID:{userID} не владеет.",targetMessage.Id,userId);
                return ((string)null).ToResult("Указанное сообщение не принадлежит вам");
            }
            targetMessage.Text = request.NewText;
            messageRep.UpdateMessage(targetMessage);
            await messageRep.Save();
            return "Успешно изменено сообщение.".ToResult("тут всё должно быть нормально.");
        }
        public async Task<Result<string, string>> DeleteMessage(int messageId, int userId)
        {
            var targetMessage = messageRep.GetMessage(messageId);
            if (targetMessage.SenderId != userId)
            {
                logger?.LogWarning("Получен запрос на удаление сообщения ID:{MessageId} которым пользователь ID:{userId} не владеет.",messageId,userId);
                return ((string)null).ToResult("Указанное сообщение не принадлежит вам");
            }
            messageRep.DeleteMessage(targetMessage);
            await messageRep.Save();
            return "Успешно удалено сообщение.".ToResult("Тут всё должно быть номарльно");
        }
    }
}
