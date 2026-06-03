using CSharpFunctionalExtensions;
using MessageService.DTO;
using MessageService.Models;

namespace MessageService.Abstractions
{
    public interface IMessageService
    {
        List<Dialog> GetDialogsPage(int userId, int page);
        Dialog? GetDialog(int dialogId);
        Dialog? GetDialog(int targetUserId, int userId);
        List<Message> GetMessagesPageFromDialog(int targetUserId, int userId,int page);
        Task<Message> CreateMessage(CreateMessageRequest request, int userId);
        Task<Result<string, string>> UpdateMessage(UpdateMessageRequest request,int userId);
        Task<Result<string, string>> DeleteMessage(int messageId, int userId);
    }
}
