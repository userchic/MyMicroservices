using Microsoft.EntityFrameworkCore;
using AuthService.Abstractions;
using AuthService.DataBase;
using AuthService.Models;

namespace AuthService.Repositories
{
    public class UsersRepository : IUserRepository
    {
        UserContext _context;
        public UsersRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<User?> Get(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(user=>user.Login==login);
        }
        public async Task<User> Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
