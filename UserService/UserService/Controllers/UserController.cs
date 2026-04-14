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
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            logger.LogInformation("Я работаю");
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
            return Json(new { errors = result.Errors.Select(error=>error.ToString()) });
        }
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
            return Json(new { errors = result.Errors.Select(error => error.ToString()) });
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
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
        [HttpGet]
        public async Task<IActionResult> GetProfileById(int userId)
        {
            if (userId<=-1)
            {
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
        [HttpGet]
        public async Task<IActionResult> GetProfiles(string? login, string? name, string? surname, string? fatname)
        {
            return Json(await _userService.GetProfiles(login, name, surname, fatname));
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangeProfile(ChangeProfileRequest request)
        {
            var getUserResult =  await GetUser();
            if(getUserResult.IsFailure)
            {
                return Json(new { error = "Не найден изменяемый пользователь" });
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

        private async Task<Result<User,string>> GetUser() => await _userService.GetProfile(User.Identity.Name);
    }
}
