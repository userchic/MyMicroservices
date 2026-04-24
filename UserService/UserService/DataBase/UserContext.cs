using Microsoft.EntityFrameworkCore;
using AuthService.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthService.DataBase
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options): base(options)
        {
            //Database.EnsureDeleted();
            bool isCreated = Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
    }
}
