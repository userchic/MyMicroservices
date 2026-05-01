using CommentsService.Dto;
using FluentValidation;

namespace CommentsService.Validators
{
    public class CreateCommentRequestValidator:AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentRequestValidator()
        {
            RuleFor((request) => request.PostId).GreaterThan(-1).WithMessage("Номер поста не может быть меньше 0");
            RuleFor((request) => request.Text).Length(10, 1000).WithMessage("Длина текста комментария должна быть от 10 до 1000 символов");
        }
    }
}
