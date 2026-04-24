using CSharpFunctionalExtensions;
using AuthService.Abstractions;
using AuthService.DTO;
using AuthService.Models;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRep;
        ILogger? logger;
        public UserService(IUserRepository userrep,ILogger<UserService>? logger)
        {
            _userRep = userrep;
            this.logger = logger;
        }
        public async Task<Result<User, string>> Login(LoginRequest request)
        {
            var user = await _userRep.Get(request.Login);
            if (user is null)
            {
                logger?.LogWarning("При попытке логина не найден пользователь с  логином {Login}.", request.Login);
                return user.ToResult("Не найден пользователь с таким логином.");
            }
            if (user.Password != request.Password)
            {
                logger?.LogWarning("При попытке логина с  логином {Login} введен неправильный пароль {Password}.", request.Login, request.Password);
                User? response = null;
                return response.ToResult("Неправильный пароль.");
            }
            logger?.LogInformation("Успешно вошел в систему {Login}", request.Login);
            return user;
        }

        public async Task<Result<User, string>> Register(RegistryRequest request)
        {
            var user = await _userRep.Get(request.Login);
            if (user is not null)
            {
                logger?.LogWarning("При попытке регистрации указан существующий логин {Login}",request.Login);
                user = null;
                return user.ToResult("Пользователь с таким логином уже существует.");
            }
            user = await _userRep.Create(User.Create(request));
            await _userRep.Save();
            logger?.LogInformation("Успешно зарегистрирован в систему {Login}", request.Login);
            return user;
        }
        
        public async Task<Result<User, string>> GetProfile(string login)
        {
            var user = await _userRep.Get(login);
            return user.ToResult("Не найден пользователь с таким логином.");

        }

        public async Task<Result<User, string>> GetProfile(int userId)
        {
            var user = await _userRep.Get(userId);
            return user.ToResult("Не найден пользователь с таким идентификатором.");
        }

        public async Task<ICollection<User>> GetProfiles(string? login, string? name, string? surname, string? fatname)
        {
            return await _userRep.GetFiltered(login, name, surname, fatname);
        }


        public async Task<Result<User, string>> ChangeProfile(ChangeProfileRequest request, User user)
        {
            var getUserResult = await _userRep.Get(user.Login);

            if (await _userRep.Get(request.Login) is not null)
            {
                logger?.LogWarning("Попытка изменить логин пользователя {Id} на существующий логин {Login}", user.Id,request.Login);
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
            getUserResult.Birthday = updatedUser.Birthday;
            _userRep.Update(getUserResult);
            await _userRep.Save();
            logger?.LogInformation("Успешно изменен профиль с идентификатором {Id}", user.Id);
            return updatedUser;
        }
    }
}
