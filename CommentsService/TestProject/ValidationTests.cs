using CommentsService.Dto;
using CommentsService.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ValidationTests
    {
        IValidator<CreateCommentRequest> createRequestValidator = new CreateCommentRequestValidator();
        IValidator<UpdateCommentRequest> updateRequestValidator = new UpdateCommentRequestValidator();
        [Theory]
        [InlineData(-1,"dwa",2)]
        [InlineData(-1,"Super text",1)]
        [InlineData(1, "Super text", 0)]
        [InlineData(1, "SuperText", 1)]
        public void CreateCommentRequestValidationTesting(int postId,string text,int amountOfErrors)
        {
            CreateCommentRequest request = new CreateCommentRequest() 
            {
                PostId= postId,
                Text = text 
            };
            var result = createRequestValidator.Validate(request);
            Assert.Equal(amountOfErrors, result.Errors.Count());
        }
        [Theory]
        [InlineData(1,"SuperTextik",0)]
        [InlineData(0,"SuperText",1)]
        [InlineData(-1,"Slova",2)]
        [InlineData(-2, "SuperskieSlova", 1)]
        public void UpdateCommentRequestValidationTesting(int commentId, string text, int amountOfErrors)
        {
            UpdateCommentRequest request = new UpdateCommentRequest()
            {
                CommentId = commentId,
                NewText = text
            };
            var result = updateRequestValidator.Validate(request);
            Assert.Equal(amountOfErrors, result.Errors.Count());
        }
    }
}
