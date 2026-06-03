using FluentValidation;
using MessageService.DTO;

namespace MessageService.Validators
{
    public class UpdateMessageRequestValidator : AbstractValidator<UpdateMessageRequest>
    {
        public UpdateMessageRequestValidator()
        {
            RuleFor((request) => request.MessageId).GreaterThan(0).WithMessage("Идентификатор сообщения не может быть меньше 1");
            //RuleFor((request)=>request.NewText)
        }
    }
}
