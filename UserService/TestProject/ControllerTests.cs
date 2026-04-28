using AuthService.Abstractions;
using AuthService.Controllers;
using AuthService.DataBase;
using AuthService.DTO;
using AuthService.Models;
using AuthService.Repositories;
using AuthService.Services;
using AuthService.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    /// <summary>
    /// Тесты работоспособности валидации контроллеров(не только FLuentValidation)
    /// </summary>
    public class ControllerTests
    {
        //[Theory]
        //[InlineData("user1", "password", true )]
        //[InlineData("user1", "", false )]
        //[InlineData("user2", "password", true )]
        //public void Login_CheckingValidationIsActive_ResultIsCorrect(string login, string password, bool mustBeSuccessful)
        //{
        //    ImplyTest((context, controller) =>
        //    {
        //        // Arrange
        //        LoginRequest request = new LoginRequest()
        //        {
        //            Login = login,
        //            Password = password
        //        };

        //        //Act
        //        JsonResult result = (JsonResult)controller.Login(request).Result;

        //        //Assert
        //        Assert.Equal(result.Value, mustBeSuccessful);
        //        //Clean
        //        context.Users.Remove(registerResult);
        //        context.SaveChanges();
        //    });
        //}


        private void ImplyTest(Action<UserContext, UserController> test)
        {
            DbContextOptionsBuilder<UserContext> optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            DbContextOptions<UserContext> options = optionsBuilder.UseInMemoryDatabase("testDb").Options;
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider();
            UserContext context = new UserContext(options);
            IUserRepository repository = new UsersRepository(context);
            IUserService service = new UserService(repository, null);
            MemoryCacheOptions cacheOptions = new MemoryCacheOptions();
            UserController controller = new UserController(service, new LoginRequestValidator(), new RegistryRequestValidator(), new ChangeProfileRequestValidator(), null, new MemoryCache(cacheOptions));

            test(context, controller);
        }
    }
}
