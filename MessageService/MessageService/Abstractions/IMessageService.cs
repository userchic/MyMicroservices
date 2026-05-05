using CSharpFunctionalExtensions;
using MessageService.DTO;
using MessageService.Models;

namespace MessageService.Abstractions
{
    public interface IMessageService
    {
        List<Message> GetMessagesPageFromDialog(int dialogId, int userId,int page);
        Task<Message> CreateMessage(CreateMessageRequest request, int userId);
        Task<Result<string, string>> UpdateMessage(UpdateMessageRequest request,int userId);
        Task<Result<string, string>> DeleteMessage(int messageId, int userId);
    }
}
