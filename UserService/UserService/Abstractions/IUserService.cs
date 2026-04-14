using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using AuthService.DTO;
using AuthService.Models;
using System.Security;

namespace AuthService.Abstractions
{
    public interface IUserService
    {
        Task<Result<User, string>> GetProfile(string login);
        Task<Result<User, string>> GetProfile(int userId);
        Task<ICollection<User>> GetProfiles(string? login,string? name,string? surname, string? fatname);

        Task<Result<User, string>> ChangeProfile(ChangeProfileRequest request,User profile);
        Task<Result<User,string>> Login(LoginRequest request);
        Task<Result<User, string>> Register(RegistryRequest request);
    }
}
