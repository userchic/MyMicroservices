using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using AuthService.DTO;
using AuthService.Models;

namespace AuthService.Abstractions
{
    public interface IUserService
    {
        Task<Result<User, string>> GetProfile(string login);
        Task<Result<User, string>> ChangeProfile(ChangeProfileRequest request,User profile);
        Task<Result<User,string>> Login(LoginRequest request);
        Task<Result<User, string>> Register(RegistryRequest request);
    }
}
