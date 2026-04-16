using AuthService.Abstractions;
using AuthService.DTO;
using AuthService.Models;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace AuthService.Controllers
{
    /// <response code="200"> Возвращает только этот код </response> 
    [ApiController]
    [Route("/[controller]/[action]")]
    public class UserController : Controller
    {
        IUserService _userService;
        IValidator<LoginRequest> loginValidator;
        IValidator<RegistryRequest> registryValidator;
        IValidator<ChangeProfileRequest> changeProfileValidator;
        ILogger logger;
        IMemoryCache usersCache;
        public UserController(IUserService userservice, IValidator<LoginRequest> loginvalidator, IValidator<RegistryRequest> registryvalidator, IValidator<ChangeProfileRequest> changeprofilevalidator,
            ILogger<UserController> logger,IMemoryCache cache)
        {
            _userService = userservice;
            loginValidator = loginvalidator;
            registryValidator = registryvalidator;
            changeProfileValidator = changeprofilevalidator;
            this.logger = logger;
            usersCache = cache;
        }
        /// <summary>
        /// Выполняет вход в систему.
        /// </summary>
        /// <param name="request">Логин и пароль</param>
        /// <returns>Токен и нового пользователя либо error или errors с ошибками</returns>
        /// <remarks>
        ///     Возвращаемый токен надобно установить в куки. Но лучше в заголовок Authorization. 
        ///     Поскольку заголовок этот уничтожается прокси - предлагается использовать заголовок "myauth".
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await loginValidator.ValidateAsync(request);
            if (result.IsValid)
            {
                var loginResult = await _userService.Login(request);
                if (loginResult.IsSuccess)
                {
                    string token = CreateToken(loginResult.Value);
                    return Json(new {token=token,value= loginResult.Value });
                }
                else
                    return Json(new { error = loginResult.Error });
            }
            logger.LogWarning("Ошибки валидации запроса на вход в систему {Count}шт", result.Errors.Count);
            return Json(new { errors = result.Errors.Select(error=>error.ToString()) });
        }
        /// <summary>
        /// Выполнит регистрацию в системе
        /// </summary>
        /// <param name="request"> Логин, пароль, ФИО, Email и дата рождения</param>
        /// <returns>Токен и нового пользователя либо error или errors с ошибками</returns>
        /// <remarks>
        ///     Параметры валидируются
        ///     Возвращаемый токен надобно установить в куки. Но лучше в заголовок Authorization. 
        ///     Поскольку заголовок этот уничтожается прокси - предлагается использовать заголовок "myauth".
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Register(RegistryRequest request)
        {
            var result = await registryValidator.ValidateAsync(request);
            if (result.IsValid)
            {
                var loginResult = await _userService.Register(request);
                if (loginResult.IsSuccess)
                {
                    string token = CreateToken(loginResult.Value);
                    return Json(new {token=token,value= loginResult.Value });
                }
                else
                    return Json(new { error = loginResult.Error });
            }
            logger.LogWarning("Ошибки валидации запроса на регистрацию в систему {Count}шт", result.Errors.Count);
            return Json(new { errors = result.Errors.Select(error => error.ToString()) });
        }
        /// <summary>
        /// Запрашивает профиль пользователя
        /// </summary>
        /// <param name="login"> Логин запрашиваемого пользователя</param>
        /// <returns>Профиль, либо error</returns>
        /// <remarks>Параметры валидируются</remarks>
        [HttpGet]
        public async Task<IActionResult> GetProfile(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                logger.LogWarning("Ошибки валидации запроса профиля по Login - {Count}шт", 1);
                return Json(new { error = "Логин не введен, введите логин" });
            }
            User cacheProfile;
            bool foundInCache=usersCache.TryGetValue(login, out cacheProfile);
            if (foundInCache)
            {
                return Json(cacheProfile);
            }
            else
            {
                var loginResult = await _userService.GetProfile(login);
                if (loginResult.IsSuccess)
                {
                    usersCache.Set(login, loginResult.Value,TimeSpan.FromMinutes(5));
                    return Json(loginResult.Value);
                }
                else
                    return Json(new { error = loginResult.Error });
            }
        }
        /// <summary>
        /// Запрашивает профиль пользователя
        /// </summary>
        /// <param name="request"> Id запрашиваемого пользователя</param>
        /// <returns>Профиль, либо error</returns>
        /// <remarks>Параметры валидируются</remarks>
        [HttpGet]
        public async Task<IActionResult> GetProfileById(int userId)
        {
            if (userId<=-1)
            {
                logger.LogWarning("Ошибки валидации запроса профиля по Id - {Count}шт", 1);
                return Json(new { error = "Идентификатор не может быть меньше 0" });
            }
            var loginResult = await _userService.GetProfile(userId);
            if (loginResult.IsSuccess)
            {
                return Json(loginResult.Value);
            }
            else
                return Json(new { error = loginResult.Error });
        }
        /// <summary>
        /// Запрашивает профили пользователей, может выдать всех пользователей(нет пагинации).
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="name">Имя</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="fatname">Отчество</param>
        /// <returns>Профиль, либо error</returns>
        /// <remarks>Нет валидации, возвращается только список. Параметры не обязательны.</remarks>

        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProfiles(string? login, string? name, string? surname, string? fatname)
        {
            return Json(await _userService.GetProfiles(login, name, surname, fatname));
        }
        /// <summary>
        /// Выполняет редактирование профиля.
        /// </summary>
        /// <param name="request"> Логин, имя, фамилия, отчество, Email, дата рождения и Login с паролем измененного пользователя.</param>
        /// <returns>Измененный профиль, либо error или errors с ошибками</returns>
        /// <remarks>Здесь нужен Header myauth с токеном Bearer.</remarks>
        [HttpPut]
        public async Task<IActionResult> ChangeProfile(ChangeProfileRequest request)
        {
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            var getUserResult = await _userService.GetProfile(userId.Value);
            if (getUserResult.IsFailure)
            {
                logger.LogWarning("При попытке изменить профиль получен не существующий Id - {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не соответствует какому либо пользователю" });
            }
            var result = await changeProfileValidator.ValidateAsync(request);
            if (result.IsValid)
            {
                var loginResult = await _userService.ChangeProfile(request,getUserResult.Value);
                if (loginResult.IsSuccess)
                {
                    return Json(loginResult.Value);
                }
                else
                    return Json(new { error = loginResult.Error });
            }
            logger.LogWarning("Ошибки валидации запроса на редактирование профиля {Login} - {Count}шт", getUserResult.Value.Login,result.Errors.Count);
            return Json(new { errors = result.Errors.Select(error => error.ToString()) });
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,user.Login),
                new Claim("UserId",user.Id.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Bearer");
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claimsIdentity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            return encodedJwt;
        }
        private int? GetUserId()
        {
            string token = HttpContext.Request.Headers.FirstOrDefault(header => header.Key.ToLower() == "myauth").Value;
            return GetUserIdFromToken(DecipherToken(token));
        }
        private JwtSecurityToken DecipherToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }
        private int? GetUserIdFromToken(JwtSecurityToken token)
        {
            try
            {
                return int.Parse(token.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value);
            }
            catch
            {
                logger.LogError("Кажется получен некорректный userId или его нет");
                return null;
            }
        }
        private async Task<Result<User,string>> GetUser() => await _userService.GetProfile(User.Identity.Name);
    }
}
