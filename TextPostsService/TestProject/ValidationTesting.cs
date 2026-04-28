using FluentValidation;
using TextPostsService.DTO;
using TextPostsService.Validators;

namespace TestProject
{
    public class ValidationTesting
    {

        IValidator<CreatePostRequest> createPostRequestValidator = new CreatePostRequestValidator();
        IValidator<UpdatePostRequest> updatePostRequestValidator = new UpdatePostRequestValidator();


        [Theory]
        [InlineData("Post",0)]
        [InlineData("",0)]
        public void CreatePostValidationTest(string text,int amountOfErrors)
        {
            CreatePostRequest request = new CreatePostRequest()
            {
                Text=text,
            };
            var result = createPostRequestValidator.Validate(request);
            Assert.Equal(amountOfErrors, result.Errors.Count);
        }
        [Theory]
        [InlineData("Post", 0,1)]
        [InlineData("Post", 1,0)]
        [InlineData("", 1,0)]
        [InlineData("", 0,1)]
        public void UpdatePostValidationTest(string text, int id, int amountOfErrors)
        {
            UpdatePostRequest request = new UpdatePostRequest()
            {
                Text = text,
                Id=id
            };
            var result = updatePostRequestValidator.Validate(request);
            Assert.Equal(amountOfErrors, result.Errors.Count);
        }
    }
}