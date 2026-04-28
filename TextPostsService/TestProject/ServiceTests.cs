using Microsoft.EntityFrameworkCore;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextPostsService.Abstractions;
using TextPostsService.DataBase;
using TextPostsService.DTO;
using TextPostsService.Repositories;
using TextPostsService.Services;

namespace TestProject
{
    public class ServiceTests
    {
        [Theory]
        [InlineData("text")]
        public void CreatePost_CreateAndCheckPostExists_SuccsessfullyFindAPost(string text)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreatePostRequest request = new CreatePostRequest()
                {
                    Text = text
                };

                //Act
                var result = service.CreatePost(request, 1).Result;
                var newPost = service.GetPost(result.Value.Id).Result;

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal(result.Value, newPost.Value);

                //Clean
                if (result.IsSuccess)
                {
                    context.Posts.Remove(result.Value);
                    context.SaveChanges();
                }
            });
        }
        [Theory]
        [InlineData("oldText","newText")]
        public void UpdatePost_CreateUpdateAndGetPost_Success(string oldText,string newText)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreatePostRequest request = new CreatePostRequest()
                {
                    Text = oldText
                };

                //Act
                var createResult = service.CreatePost(request, 1).Result;
                UpdatePostRequest updateRequest = new UpdatePostRequest()
                {
                    Text = newText,
                    Id = createResult.Value.Id,
                };
                var updateResult = service.UpdatePost(updateRequest, 1).Result;
                var getResult = service.GetPost(createResult.Value.Id).Result;

                //Assert
                Assert.True(createResult.IsSuccess);
                Assert.True(updateResult.IsSuccess);
                Assert.True(getResult.IsSuccess);
                Assert.Equal(updateResult.Value, getResult.Value);

                //Clean
                if (getResult.IsSuccess)
                {
                    context.Posts.Remove(getResult.Value);
                    context.SaveChanges();
                }
            });
        }
        [Theory]
        [InlineData("text")]
        public void DeletePost_CreateDeleteAndGetPost_Success(string text)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreatePostRequest request = new CreatePostRequest()
                {
                    Text = text
                };

                //Act
                var createResult = service.CreatePost(request, 1).Result;
                var deleteResult = service.DeletePost(createResult.Value.Id,1).Result;
                var getResult = service.GetPost(createResult.Value.Id).Result;

                //Assert
                Assert.True(createResult.IsSuccess);
                Assert.True(deleteResult.IsSuccess);
                Assert.False(getResult.IsSuccess);
            });
        }
        private void ImplyTest(Action<TextPostContext, TextPostService> test)
        {
            DbContextOptionsBuilder<TextPostContext> optionsBuilder = new DbContextOptionsBuilder<TextPostContext>();
            DbContextOptions<TextPostContext> options = optionsBuilder.UseInMemoryDatabase("testDb").Options;
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider();
            TextPostContext context = new TextPostContext(options);
            ITextPostRepository repository = new TextPostRepository(context);

            test(context, new TextPostService(repository, null));
        }
    }
}
