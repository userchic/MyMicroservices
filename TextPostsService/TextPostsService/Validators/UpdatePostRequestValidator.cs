using FluentValidation;
using TextPostsService.DTO;

namespace TextPostsService.Validators
{
    public class UpdatePostRequestValidator:AbstractValidator<UpdatePostRequest>
    {
        public UpdatePostRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("Не указан идентификатор поста, укажите его.");
            RuleFor(request => request.Text).Length(0, 1000).WithMessage("Текст поста должен быть непустой строкой длиной до 1000 символов.");
        }
    }
}
