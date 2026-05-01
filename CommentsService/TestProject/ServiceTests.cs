using CommentsService.Abstractions;
using CommentsService.DataBase;
using CommentsService.Dto;
using CommentsService.Repository;
using CommentsService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog.Extensions.Logging;

namespace TestProject
{
    public class ServiceTests
    {
        [Theory]
        [InlineData("Ogromniy text",1,1)]
        [InlineData("Ogromniy text2",1,1)]
        [InlineData("Ogromniy text3",2,2)]
        [InlineData("Ogromniy text4",2,2)]
        public void CreateComment_CreateAndGetComment_SuccessfullyGetComment(string text,int postId,int userId)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreateCommentRequest request = new CreateCommentRequest()
                {
                    PostId = postId,
                    Text = text
                };
                
                //Act
                var result = service.AddComment(request, userId).Result;
                var getResult = service.GetComment(result.Id);

                //Assert
                Assert.Equal(getResult, result);

                //Clean
                context.Comments.Remove(getResult);
                context.SaveChanges();
            });
        }
        [Theory]
        [InlineData("Ogromniy text", 1, 1,"Malenkiy text")]
        [InlineData("Ogromniy text2", 1, 1, "Malenkiy text2")]
        [InlineData("Ogromniy text3", 2, 2, "Malenkiy text3")]
        [InlineData("Ogromniy text4", 2, 2, "Malenkiy text4")]
        public void UpdateComment_CreateUpdateAndGetComment_SuccessfullyGetUpdatedCommentComment(string text, int postId, int userId,string NewText)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreateCommentRequest request = new CreateCommentRequest()
                {
                    PostId = postId,
                    Text = text
                };

                //Act
                var result = service.AddComment(request, userId).Result;
                UpdateCommentRequest updateRequest = new UpdateCommentRequest()
                {
                    CommentId = result.Id,
                    NewText = NewText
                };
                var updateResult = service.UpdateComment(updateRequest, userId).Result;
                var getResult = service.GetComment(result.Id);

                //Assert
                Assert.True(updateResult.IsSuccess);
                Assert.Equal(getResult, result);

                //Clean
                context.Comments.Remove(getResult);
                context.SaveChanges();
            });
        }
        [Theory]
        [InlineData("Ogromniy text", 1, 1)]
        [InlineData("Ogromniy text2", 1, 1)]
        [InlineData("Ogromniy text3", 2, 2)]
        [InlineData("Ogromniy text4", 2, 2)]
        public void DeleteComment_CreateDeleteAndGetComment_GetNoComment(string text, int postId, int userId)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreateCommentRequest request = new CreateCommentRequest()
                {
                    PostId = postId,
                    Text = text
                };

                //Act
                var result = service.AddComment(request, userId).Result;
                var deleteResult = service.DeleteComment(userId, result.Id).Result;
                var getResult = service.GetComment(result.Id);

                //Assert
                Assert.True(deleteResult.IsSuccess);
                Assert.Null(getResult);
            });
        }
        public void ImplyTest(Action<CommentsContext, CommentService> test)
        {
            DbContextOptionsBuilder<CommentsContext> optionsBuilder = new DbContextOptionsBuilder<CommentsContext>();
            DbContextOptions<CommentsContext> options = optionsBuilder.UseInMemoryDatabase("testDb").Options;
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider();
            CommentsContext context = new CommentsContext(options);
            ICommentsRepository repository = new CommentsRepository(context);

            test(context, new CommentService(repository, null));
        }
    }
}