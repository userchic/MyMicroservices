using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Serilog.Extensions.Logging;
using SubscriptionsService.Abstractions;
using SubscriptionsService.DataBase;
using SubscriptionsService.Repositories;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Subscribe_TargetEqualUser_ErrorMessage()
        {
            ImplyTest((context, service) =>
            {
                //Act
                var result = service.Subscribe(1, 1).Result;

                //Assert
                Assert.True(result.IsFailure);
            });
        }
        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        [InlineData(1, 3)]
        [InlineData(3, 1)]
        public void Subscribe_SubscribeAndFindSubscription_SuccssfullyFindSubscription(int userId, int targetId)
        {
            ImplyTest((context, service) =>
            {
                //Act
                var result = service.Subscribe(userId, targetId).Result;
                var getResult = service.GetSubscribers(targetId);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.True(getResult.Any((subscription) => subscription.SubscriberId == userId && subscription.PosterId == targetId));

                //Clean
                var newSubscription = getResult.FirstOrDefault((subscription) => subscription.SubscriberId == userId && subscription.PosterId == targetId);
                context.Subscriptions.Remove(newSubscription);
                context.SaveChanges();
            });
        }
        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        [InlineData(1, 3)]
        [InlineData(3, 1)]
        public void Unsubscribe_SubscribeUnsubscribeAndFindSubscription_FindNoSubscription(int userId,int targetId)
        {
            ImplyTest((context, service) =>
            {
                //Act
                var result = service.Subscribe(userId, targetId).Result;
                var unsubscribeResult = service.UnSubscribe(userId, targetId).Result;
                var getResult = service.GetSubscribers(targetId);

                //Assert
                Assert.True(result.IsSuccess);
                Assert.True(unsubscribeResult.IsSuccess);
                Assert.False(getResult.Any((subscription) => subscription.SubscriberId == userId && subscription.PosterId == targetId));

            });
        }
        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        [InlineData(1, 3)]
        [InlineData(3, 1)]
        public void Unsubscribe_UnsubscribeWhenNoSubscription_ErrorMessage(int userId, int targetId)
        {
            ImplyTest((context, service) =>
            {
                //Act
                var unsubscribeResult = service.UnSubscribe(userId, targetId).Result;

                //Assert
                Assert.True(unsubscribeResult.IsFailure);

            });
        }
        private void ImplyTest(Action<SubscriptionsContext, SubscriptionsService.Services.SubscriptionsService> test)
        {
            DbContextOptionsBuilder<SubscriptionsContext> optionsBuilder = new DbContextOptionsBuilder<SubscriptionsContext>();
            DbContextOptions<SubscriptionsContext> options = optionsBuilder.UseInMemoryDatabase("testDb").Options;
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider();
            SubscriptionsContext context = new SubscriptionsContext(options);
            ISubscriptionRepository repository = new SubscriptionsRepository(context);

            test(context, new SubscriptionsService.Services.SubscriptionsService(repository, null));
        }
    }
}