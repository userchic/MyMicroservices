using CSharpFunctionalExtensions;
using AuthService.Abstractions;
using AuthService.DTO;
using AuthService.Models;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRep;
        public UserService(IUserRepository userrep)
        {
            _userRep = userrep;
        }

        public async Task<Result<User, string>> ChangeProfile(ChangeProfileRequest request,User user)
        {
            var getUserResult = await _userRep.Get(user.Login);

            if(await _userRep.Get(request.Login)is not null)
            {
                User nullUser = null;
                return nullUser.ToResult("Пользователь с этим логином уже существует");
            }
            User updatedUser = User.Create(request);
            updatedUser.Id = getUserResult.Id;
            getUserResult.Login = updatedUser.Login;
            getUserResult.Password = updatedUser.Password;
            getUserResult.Name = updatedUser.Name;
            getUserResult.Surname = updatedUser.Surname;
            getUserResult.Fatname = updatedUser.Fatname;
            getUserResult.Birthday = updatedUser.Birthday.ToUniversalTime();
            _userRep.Update(getUserResult);
            await _userRep.Save();
            return updatedUser;
        }

        public async Task<Result<User, string>> GetProfile(string login)
        {
            var user = await _userRep.Get(login);
            return user.ToResult("Не найден пользователь с таким логином.");

        }

        public async Task<Result<User, string>> Login(LoginRequest request)
        {
            var user = await _userRep.Get(request.Login);
            if(user is null)
            {
                return user.ToResult("Не найден пользователь с таким логином.");
            }
            if (user.Password != request.Password)
            {
                User? response=null;
                return response.ToResult("Неправильный пароль.");
            }
            else
            {
                return user;
            }
        }

        public async Task<Result<User, string>> Register(RegistryRequest request)
        {
            var user = await _userRep.Get(request.Login);
            if (user is not null)
            {
                user = null;
                return user.ToResult("Пользователь с таким логином уже существует.");
            }
            user = await _userRep.Create(User.Create(request));
            await _userRep.Save();
            return user;
        }
    }
}
