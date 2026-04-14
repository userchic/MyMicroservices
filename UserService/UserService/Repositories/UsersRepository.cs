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
            return await _context.Users.FirstOrDefaultAsync(user => user.Login == login);
        }
        public async Task<User?> Get(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
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

        public async Task<ICollection<User>> GetFiltered(string? login, string? name, string? surname, string? fatname)
        {
            IQueryable<User> query = _context.Users.AsQueryable();
            if (login is not null)
                query=query.Where(user => user.Login.Contains(login));
            if (name is not null)
                query = query.Where(user => user.Name.Contains(name));
            if (surname is not null)
                query = query.Where(user => user.Surname.Contains(surname));
            if (fatname is not null)
                query = query.Where(user => user.Fatname.Contains(fatname));
            return await query.ToListAsync();
        }
    }
}
