using AuthService.Extensions;
using FluentValidation;
using NotificationsService.DTO;

namespace NotificationsService.Validators
{
    public class UpdateNotificationRuleRequestValidator:AbstractValidator<UpdateNotificationRulesRequest>
    {
        public UpdateNotificationRuleRequestValidator()
        {
            RuleFor((request) => request).Custom((request, context) =>
            {
                if (request.NotificationService != "mail.ru" || !request.InternalServiceIdentificator.IsValidEmail())
                    context.AddFailure(new FluentValidation.Results.ValidationFailure("object", "Введен не существующий вариант уведомления или неправильный внутренний идентификатор сервиса уведомлений"));
            });
        }
    }
}
