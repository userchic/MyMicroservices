using FluentValidation;
using TextPostsService.DTO;

namespace TextPostsService.Validators
{
    public class CreatePostRequestValidator:AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(request => request.Text).Length(0, 1000).WithMessage("Текст поста должен быть непустой строкой длиной до 1000 символов.");
        }
    }
}
