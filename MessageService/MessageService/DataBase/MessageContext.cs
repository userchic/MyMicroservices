using MessageService.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageService.DataBase
{
    public class MessageContext:DbContext
    {
        public MessageContext(DbContextOptions options) : base(options)
        {
            bool isCreated = Database.EnsureCreated();
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Dialog> Dialogues { get; set; }
    }
}
