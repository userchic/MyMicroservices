using FluentValidation;
using MessageService.DTO;

namespace MessageService.Validators
{
    public class CreateMessageRequestValidator:AbstractValidator<CreateMessageRequest>
    {
        public CreateMessageRequestValidator()
        {
            RuleFor((request) => request.RecieverId).GreaterThan(0).WithMessage("Идентификатор получателя не может быть меньше 1");
            //RuleFor((request)=>request.Text)
        }
    }
}
