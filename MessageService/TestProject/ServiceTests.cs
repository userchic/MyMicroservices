using MessageService.Abstractions;
using MessageService.DataBase;
using MessageService.DTO;
using MessageService.Repositories;
using MessageService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog.Extensions.Logging;

namespace TestProject
{
    public class ServiceTests
    {
        [Theory]
        [InlineData(2,"text")]
        [InlineData(2,"")]
        [InlineData(2,"dwa")]
        [InlineData(2, "dwa")]
        public void CreateMessage_CreateAndGetMessage_SuccessfullyGetMessage(int receiverId,string text)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreateMessageRequest request = new CreateMessageRequest()
                {
                    RecieverId=receiverId,
                    Text = text
                };
                //Act
                var createResult = service.CreateMessage(request, 1).Result;
                var getResult = service.GetMessagesPageFromDialog(createResult.DialogId, 1, 1);
                //Assert
                Assert.Equal(createResult.Text, text);
                Assert.Equal(getResult[0].Text, createResult.Text);
                Assert.Equal(getResult[0].Id, createResult.Id);
                Assert.Equal(getResult[0].DialogId, createResult.DialogId);
                Assert.Equal(getResult[0].SenderId, createResult.SenderId);
                //Cleanup
                context.Messages.Remove(getResult[0]);
                context.SaveChanges();
            });
        }
        [Theory]
        [InlineData(2, "text","newText")]
        [InlineData(2, "","text2")]
        [InlineData(2, "dwa","tri")]
        [InlineData(2, "dwa","odin")]
        public void UpdateMessage_CreateUpdateAndGetMessage_SuccessfullyGetMessage(int receiverId, string text,string newText)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreateMessageRequest createRequest = new CreateMessageRequest()
                {
                    RecieverId = receiverId,
                    Text = text
                };
                //Act
                var createResult = service.CreateMessage(createRequest, 1).Result;
                UpdateMessageRequest updateRequest = new UpdateMessageRequest()
                {
                    MessageId = createResult.Id,
                    NewText = newText
                };
                var updateResult = service.UpdateMessage(updateRequest, 1).Result;
                var getResult = service.GetMessagesPageFromDialog(createResult.DialogId, 1, 1);
                //Assert
                Assert.True(updateResult.IsSuccess);
                Assert.Equal(updateRequest.NewText, getResult[0].Text);
                Assert.Equal(getResult[0].Id, createResult.Id);
                Assert.Equal(getResult[0].DialogId, createResult.DialogId);
                Assert.Equal(getResult[0].SenderId, createResult.SenderId);
                //Cleanup
                context.Messages.Remove(getResult[0]);
                context.SaveChanges();
            });
        }
        [Theory]
        [InlineData(2, "text", "newText")]
        [InlineData(2, "", "text2")]
        [InlineData(2, "dwa", "tri")]
        [InlineData(2, "dwa", "odin")]
        public void DeleteMessage_CreateDeleteAndGetMessage_GetNoMessages(int receiverId, string text, string newText)
        {
            ImplyTest((context, service) =>
            {
                //Arrange
                CreateMessageRequest createRequest = new CreateMessageRequest()
                {
                    RecieverId = receiverId,
                    Text = text
                };
                //Act
                var createResult = service.CreateMessage(createRequest, 1).Result;
                UpdateMessageRequest updateRequest = new UpdateMessageRequest()
                {
                    MessageId = createResult.Id,
                    NewText = newText
                };
                var deleteResult = service.DeleteMessage(createResult.Id, 1).Result;
                var getResult = service.GetMessagesPageFromDialog(createResult.DialogId, 1, 1);
                //Assert
                Assert.Equal(createResult.Text, text);
                Assert.True(deleteResult.IsSuccess);
                Assert.Equal(getResult.Count, 0);
            });
        }
        private void ImplyTest(Action<MessageContext, MessageService.Services.MessageService> test)
        {
            DbContextOptionsBuilder<MessageContext> optionsBuilder = new DbContextOptionsBuilder<MessageContext>();
            DbContextOptions<MessageContext> options = optionsBuilder.UseInMemoryDatabase("testDb").Options;
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider();
            MessageContext context = new MessageContext(options);
            IMessageRepository messageRepository = new MessageRepository(context);
            IDialogRepository dialogRepository = new DialogRepository(context);

            test(context, new MessageService.Services.MessageService(messageRepository, dialogRepository, null));
        }
    }
}