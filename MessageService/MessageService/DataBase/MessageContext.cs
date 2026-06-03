using MessageService.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Dialog>().HasMany(dialog => dialog.Messages).WithOne(message => message.Dialog).HasForeignKey(message=>message.DialogId);
        }
    }
}
