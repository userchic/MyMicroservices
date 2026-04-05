using AuthService.Models;

namespace AuthService.Abstractions
{
    public interface IUserRepository: IRepository
    {
        Task<User?> Get(string Login);
        Task<ICollection<User>> GetFiltered(string? login, string? name, string? surname, string? fatname);
        Task<User> Create(User user);
        void Update(User user);

    }
}
