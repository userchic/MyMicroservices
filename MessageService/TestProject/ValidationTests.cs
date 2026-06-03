using FluentValidation;
using MessageService.DTO;
using MessageService.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ValidationTests
    {
        IValidator<CreateMessageRequest> createMessageValidator=new CreateMessageRequestValidator();
        IValidator<UpdateMessageRequest> updateMessageValidator= new UpdateMessageRequestValidator();
        [Theory]
        [InlineData(1,"",0)]
        [InlineData(1, "da",0)]
        [InlineData(1, "abc", 0)]
        [InlineData(1, "text", 0)]
        [InlineData(1, " ", 0)]
        [InlineData(1, null, 0)]
        [InlineData(-1, null, 1)]
        [InlineData(-2, null, 1)]
        public void CreateMessageRequestValidationTesting(int receiverId,string text,int mistakeAmount)
        {
            CreateMessageRequest request = new CreateMessageRequest()
            {
                RecieverId=receiverId,
                Text=text
            };
            var result = createMessageValidator.Validate(request);
            Assert.Equal(mistakeAmount, result.Errors.Count);
        }
        [Theory]
        [InlineData(1, "", 0)]
        [InlineData(1, "da", 0)]
        [InlineData(1, "abc", 0)]
        [InlineData(1, "text", 0)]
        [InlineData(1, " ", 0)]
        [InlineData(1, null, 0)]
        [InlineData(-1, null, 1)]
        [InlineData(-2, null, 1)]
        public void UpdateMessageRequestValidationTesting(int messageId, string newText, int mistakeAmount)
        {
            UpdateMessageRequest request = new UpdateMessageRequest()
            {
                MessageId=messageId,
                NewText=newText
            };
            var result = updateMessageValidator.Validate(request);
            Assert.Equal(mistakeAmount, result.Errors.Count);
        }
    }
}
