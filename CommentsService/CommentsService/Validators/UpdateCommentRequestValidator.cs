using CommentsService.Dto;
using FluentValidation;

namespace CommentsService.Validators
{
    public class UpdateCommentRequestValidator:AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentRequestValidator()
        {
            RuleFor((request)=>request.NewText).Length(10, 1000).WithMessage("Длина текста комментария должна быть от 10 до 1000 символов");
            RuleFor((request)=>request.CommentId).GreaterThan(-1).WithMessage("Номер комментария не может быть меньше 0");
        }
    }
}
