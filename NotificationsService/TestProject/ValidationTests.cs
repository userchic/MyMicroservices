using FluentValidation;
using NotificationsService.DTO;
using NotificationsService.Validators;

namespace TestProject
{
    public class ValidationTests
    {
        IValidator<CreateNotificationRulesRequest> createNotificationRuleRequestValidator = new CreateNotificationRuleRequestValidator();
        IValidator<UpdateNotificationRulesRequest> updateNotificationRuleRequestValidator = new UpdateNotificationRuleRequestValidator();
        [Theory]
        [InlineData("","",1)]
        [InlineData("mail.ru", "", 1)]
        [InlineData("", "r.gabzalil@mail.ru", 1)]
        public void CreateNotificationRuleValidationTest(string notificationsService ,string internalServiceIdentificator,int amountOfMistakes)
        {
            //Arrange
            CreateNotificationRulesRequest request = new CreateNotificationRulesRequest()
            {
                NotificationService = notificationsService,
                InternalServiceIdentificator = internalServiceIdentificator
            };
            //Act
            var result = createNotificationRuleRequestValidator.Validate(request);
            
            //Assert
            Assert.Equal(amountOfMistakes,result.Errors.Count());
        }
        [Theory]
        [InlineData("", "", 1)]
        [InlineData("mail.ru", "", 1)]
        [InlineData("", "r.gabzalil@mail.ru", 1)]
        public void UpdateNotificationRuleValidationTest(string notificationsService, string internalServiceIdentificator, int amountOfMistakes)
        {
            //Arrange
            UpdateNotificationRulesRequest request = new UpdateNotificationRulesRequest()
            {
                NotificationService = notificationsService,
                InternalServiceIdentificator = internalServiceIdentificator
            };
            //Act
            var result = updateNotificationRuleRequestValidator.Validate(request);

            //Assert
            Assert.Equal(amountOfMistakes, result.Errors.Count());
        }
    }
}