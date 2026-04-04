using FluentValidation;
using AuthService.Abstractions;
using AuthService.DTO;

namespace AuthService.Validators
{
    public class LoginRequestValidator:AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.Login).NotEmpty().WithMessage("Логин не введен, введите его");
            RuleFor(request => request.Password).NotEmpty().WithMessage("Пароль не введен, введите его");
        }
    }
}
