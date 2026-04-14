using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using NotificationsService.DTO;
using NotificationsService.Models;

namespace NotificationsService.Abstractions
{
    public interface IPersonalNotificationRulesService
    {
        Task<PersonalNotificationRule?> GetPersonalRule(int userId);
        Task<Result<string, string>> CreatePersonalRule(CreateNotificationRulesRequest rule, int userId);
        Task<Result<string, string>> UpdatePersonalRule(UpdateNotificationRulesRequest rule, int userId);
        Task<Result<string, string>> DeletePersonalRule(int userId);
    }
}
