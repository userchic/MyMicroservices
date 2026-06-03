using MessageService.Models;

namespace MessageService.Abstractions
{
    public interface IMessageRepository:IRepository
    {
        Message? GetMessage(int messageId);
        List<Message> GetMessagesPageFromDialog(int dialogId,int page);
        Task<Message> CreateMessage(Message newMessage);
        void UpdateMessage(Message updatedMessage);
        void DeleteMessage(Message message);
    }
}
