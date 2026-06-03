using MessageService.Models;

namespace MessageService.Abstractions
{
    public interface IDialogRepository:IRepository
    {
        List<Dialog> GetDialogsPage(int userId, int page);
        Dialog? GetDialog(int dialogId);
        Dialog? GetDialog(int user1Id,int user2Id);
        List<Dialog> GetUserDialogues(int userId, int page);
        Task<Dialog> CreateDialog(Dialog newDialog);
    }
}
