using CSharpFunctionalExtensions;
using FluentValidation;
using AuthService.DTO;

namespace AuthService.Validators
{
    public class RegistryRequestValidator:AbstractValidator<RegistryRequest>
    {
        public RegistryRequestValidator()
        {
            RuleFor(request => request.Password).Length(5, 20).WithMessage("Пароль должен быть длиной от 5 до 20 символов");
            RuleFor(request => request.Login).Length(5, 40).WithMessage("Длина логина должна быть 5-40 символов");
            RuleFor(request =>request.Name).Length(3, 50).WithMessage("Длина имени должна быть 3-50 символов");
            RuleFor(request =>request.Surname).Length(3, 50).WithMessage("Длина фамилии должна быть 3-50 символов");
            RuleFor(request =>request.Fatname).Length(0,50).WithMessage("Длина отчества должна быть до 50 символов");
            RuleFor(request => request.Birthday).LessThan(DateTime.Now).WithMessage("Дата рождения должна быть в прошлом");
        }
    }
}
