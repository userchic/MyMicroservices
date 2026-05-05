using MessageService.Abstractions;
using MessageService.DataBase;
using MessageService.Models;

namespace MessageService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        MessageContext context;
        public MessageRepository (MessageContext context)
        {
            this.context = context;
        }
        public Message? GetMessage(int messageId)
        {
            return context.Messages.FirstOrDefault((message)=>message.Id == messageId);
        }

        public List<Message> GetMessagesPageFromDialog(int dialogId, int page)
        {
            return context.Messages.Where((message) => message.DialogId == dialogId).Skip((page - 1) * 10).Take(10).ToList();
        }
        public async Task<Message> CreateMessage(Message newMessage)
        {
            await context.Messages.AddAsync(newMessage);
            return newMessage;
        }
        public void UpdateMessage(Message updatedMessage)
        {
            context.Messages.Update(updatedMessage);
        }
        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
