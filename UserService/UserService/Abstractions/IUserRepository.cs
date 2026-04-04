using AuthService.Models;

namespace AuthService.Abstractions
{
    public interface IUserRepository: IRepository
    {
        Task<User?> Get(string Login);
        Task<User> Create(User user);
        void Update(User user);
    }
}
