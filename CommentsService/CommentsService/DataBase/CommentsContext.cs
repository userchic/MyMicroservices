using CommentsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsService.DataBase
{
    public class CommentsContext:DbContext
    {
        public CommentsContext(DbContextOptions<CommentsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Comment> Comments;
    }
}
