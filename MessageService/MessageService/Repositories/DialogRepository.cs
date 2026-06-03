using MessageService.Abstractions;
using MessageService.DataBase;
using MessageService.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageService.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        MessageContext context;
        public DialogRepository(MessageContext context)
        {
            this.context = context;
        }
        public List<Dialog> GetDialogsPage(int userId, int page)
        {
            return context.Dialogues.Where((dialog) => dialog.User1Id == userId || dialog.User2Id == userId).Skip((page - 1) * 10).Take(10).ToList(); 
        }
        public Dialog? GetDialog(int dialogId)
        {
            return context.Dialogues.AsNoTracking().FirstOrDefault((dialog) => dialog.Id == dialogId);
        }
        public Dialog? GetDialog(int user1Id,int user2Id)
        {
            return context.Dialogues.AsNoTracking().FirstOrDefault((dialog)=> dialog.User1Id == user1Id && dialog.User2Id == user2Id|| dialog.User1Id == user2Id && dialog.User2Id == user1Id);
        }

        public List<Dialog> GetUserDialogues(int userId, int page)
        {
            return context.Dialogues.Where((dialog) => dialog.User1Id == userId || dialog.User2Id == userId).Skip((page - 1) * 10).Take(10).ToList();
        }
        public async Task<Dialog> CreateDialog(Dialog newDialog)
        {
            await context.Dialogues.AddAsync(newDialog);
            return newDialog;
        }
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }


    }
}
