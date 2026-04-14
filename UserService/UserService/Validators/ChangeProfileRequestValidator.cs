using FluentValidation;
using AuthService.DTO;
using AuthService.Extensions;

namespace AuthService.Validators
{
    public class ChangeProfileRequestValidator:AbstractValidator<ChangeProfileRequest>
    {
        public ChangeProfileRequestValidator()
        {
            RuleFor(request => request.Password).Length(5, 20).WithMessage("Длина Пароля должна быть 5-20 символов");
            RuleFor(request => request.Login).Length(5, 40).WithMessage("Длина логина должна быть 5-40 символов").Matches("(?!^\\d+$)^.+$").WithMessage("Логин не может быть числом");
            RuleFor(request => request.Name).Length(3, 50).WithMessage("Длина имени должна быть 3-50 символов");
            RuleFor(request => request.Surname).Length(3, 50).WithMessage("Длина фамилии должна быть 3-50 символов");
            RuleFor(request => request.Fatname).Length(0, 50).WithMessage("Длина отчества должна быть до 50 символов");
            RuleFor(request => request.Birthday).LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Дата рождения должна быть в прошлом");
            RuleFor(request => request.Email).Must<ChangeProfileRequest, string>((string email) => email.IsValidEmail() || string.IsNullOrEmpty(email)).WithMessage("Введенная строка не распознана как Email");
        }
    }
}
