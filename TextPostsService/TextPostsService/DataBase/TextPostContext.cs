using Microsoft.EntityFrameworkCore;
using TextPostsService.Model;

namespace TextPostsService.DataBase
{
    public class TextPostContext:DbContext
    {
        public TextPostContext(DbContextOptions<TextPostContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            bool isCreated = Database.EnsureCreated();
        }
        public DbSet<TextPost> Posts { get; set; }
    }
}
