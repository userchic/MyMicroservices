using Microsoft.EntityFrameworkCore;
using NotificationsService.Abstractions;
using NotificationsService.DataBase;
using NotificationsService.DTO;
using NotificationsService.Repositories;
using NotificationsService.Services;
using Serilog.Extensions.Logging;

namespace TestProject
{
    public class ServiceTests
    {
        [Theory]
        [InlineData("mail.ru","r.gabzalil@mail.ru")]
        public void CreatePersonalRule_CreateAndGetRule_SuccessfullyGetNewRule(string notificationsService, string internalServiceIdentificator)
        {
            ImplyTest((context, service)=>{
                //Arrange
                CreateNotificationRulesRequest request = new CreateNotificationRulesRequest()
                {
                    NotificationService=notificationsService,
                    InternalServiceIdentificator = internalServiceIdentificator
                };
                //Act
                var result = service.CreatePersonalRule(request, 1).Result;
                var newRule = service.GetPersonalRule(1).Result;

                //Assert
                Assert.True(result.IsSuccess);
                Assert.True(newRule.UserId == 1);
                Assert.True(newRule.InternalServiceIdentificator == request.InternalServiceIdentificator);
                Assert.True(newRule.NotificationService == request.NotificationService);

                //Clean
                context.Rules.Remove(newRule);
                context.SaveChanges();
            });
        }
        [Theory]
        [InlineData("mail.ru", "r.gabzalil@mail.ru", "", "")]
        public void UpdatePersonalRule_CreateUpdateAndGetRule_SuccessfullyGetUpdatedRule(string notificationsService, string internalServiceIdentificator,string newNotificationsService, string newinternalServiceIdentificator)
        {
            ImplyTest((context, service) => {
                //Arrange
                CreateNotificationRulesRequest request = new CreateNotificationRulesRequest()
                {
                    NotificationService = notificationsService,
                    InternalServiceIdentificator = internalServiceIdentificator
                };
                UpdateNotificationRulesRequest updateRequest = new UpdateNotificationRulesRequest()
                {
                    NotificationService = notificationsService,
                    InternalServiceIdentificator = internalServiceIdentificator
                };
                //Act
                var result = service.CreatePersonalRule(request, 1).Result;
                var updateResult = service.UpdatePersonalRule(updateRequest, 1).Result;
                var newRule = service.GetPersonalRule(1).Result;

                //Assert
                Assert.True(result.IsSuccess);
                Assert.True(updateResult.IsSuccess);
                Assert.True(newRule.UserId == 1);
                Assert.True(newRule.InternalServiceIdentificator == request.InternalServiceIdentificator);
                Assert.True(newRule.NotificationService == request.NotificationService);
                
                //Clean
                context.Rules.Remove(newRule);
                context.SaveChanges();
            });
        }
        [Theory]
        [InlineData("mail.ru", "r.gabzalil@mail.ru")]
        public void DeletePersonalRule_CreateDeleteAndGetRule_GetNoRule(string notificationsService, string internalServiceIdentificator)
        {
            ImplyTest((context, service) => {
                //Arrange
                CreateNotificationRulesRequest request = new CreateNotificationRulesRequest()
                {
                    NotificationService = notificationsService,
                    InternalServiceIdentificator = internalServiceIdentificator
                };
                //Act
                var result = service.CreatePersonalRule(request, 1).Result;
                var deleteResult = service.DeletePersonalRule(1).Result;
                var newRule = service.GetPersonalRule(1).Result;

                //Assert
                Assert.True(result.IsSuccess);
                Assert.True(deleteResult.IsSuccess);
                Assert.Null(newRule);


            });
        }
        private void ImplyTest(Action<NotificationContext,PersonalNotificationRulesService> test)
        {
            DbContextOptionsBuilder<NotificationContext> optionsBuilder = new DbContextOptionsBuilder<NotificationContext>();
            DbContextOptions<NotificationContext> options = optionsBuilder.UseInMemoryDatabase("testDb").Options;
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider();
            NotificationContext context = new NotificationContext(options);
            INotificationRulesRepository repository = new NotificationRulesRepository(context);

            test(context, new PersonalNotificationRulesService(repository, null));
        }
    }
}