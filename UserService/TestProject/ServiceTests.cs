using AuthService.Abstractions;
using AuthService.DataBase;
using AuthService.DTO;
using AuthService.Models;
using AuthService.Repositories;
using AuthService.Services;
using AuthService.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
namespace TestProject
{
    public class ServiceTests
    {
        [Theory]
        [InlineData("user1", "password", "user2", "password", false)]
        [InlineData("user1", "password", "user2", "password2", false)]
        [InlineData("user2", "password", "user1", "password2", false)]
        public void Login_AnExistingAndRequestedUser_ResultIsParameter(string existingLogin, string existingPassword, string login, string password, bool mustBeSuccessful)
        {
            ImplyTest((context, service) =>
            {
                // Arrange
                User newUser = service.Register(new RegistryRequest()
                {
                    Login = existingLogin,
                    Password = existingPassword,
                    Name = "dwafres",
                    Surname = "dwafres",
                    Fatname = "dwafres",
                    Email = "",
                    Birthday = new DateOnly(2000, 12, 31)
                }).Result.Value;
                LoginRequest request = new LoginRequest()
                {
                    Login = login,
                    Password = password
                };

                //Act
                var result = service.Login(request).Result;

                //Assert
                Assert.Equal(result.IsSuccess, mustBeSuccessful);
                //Clean
                context.Users.Remove(newUser);
                context.SaveChanges();
            });
        }
        public static TheoryData<string, string, string, string, string, string, DateOnly> RegistryCases1 = new TheoryData<string, string, string, string, string, string, DateOnly>()
        {
            {"user1","password","Iname","surname","fatname","r.gabzalil@mail.ru",new DateOnly(2027,12,31)},
            {"user1","password","Iname","surname","fatname","",new DateOnly(2027,12,31)},
        };
        [Theory, MemberData(nameof(RegistryCases1))]
        public void Register_NonExistingUser_Success(string login, string password, string name, string surname, string fatname, string email, DateOnly birthday)
        {
            ImplyTest((context, service) =>
            {
                ///Arrange
                RegistryRequest request = new RegistryRequest()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    Birthday = birthday,
                    Email = email
                };

                //Act
                var result = service.Register(request).Result;

                //Assert
                Assert.True(result.IsSuccess);
                Assert.Equal(result.Value,User.Create(request));

                //Clean
                if (result.IsSuccess)
                {
                    context.Users.Remove(result.Value);
                    context.SaveChanges();
                }
            });
        }
        [Theory, MemberData(nameof(RegistryCases1))]
        public void Register_ExistingUser_ErrorMessage(string login, string password, string name, string surname, string fatname, string email, DateOnly birthday)
        {
            ImplyTest((context, service) =>
            {
                ///Arrange
                RegistryRequest request = new RegistryRequest()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    Birthday = birthday,
                    Email = email
                };

                //Act
                var result = service.Register(request).Result;
                var resultMain = service.Register(request).Result;

                //Assert
                Assert.False(resultMain.IsSuccess);

                //Clean
                if (result.IsSuccess)
                {
                    context.Users.Remove(result.Value);
                    context.SaveChanges();
                }
            });
        }
        public static TheoryData<string, string, string, string, string, string, DateOnly, int> RegistryCases2 = new TheoryData<string, string, string, string, string, string, DateOnly, int>()
        {
            {"user1","password","Iname","surname","fatname","r.gabzalil@mail.ru",new DateOnly(2027,12,31),1 },
            {"user1","password","Iname","surname","fatname","",new DateOnly(2027,12,31),1 },
        };
        [Theory, MemberData(nameof(RegistryCases1))]
        public void GetProfile_GetExistingUserByLogin_Value(string login, string password, string name, string surname, string fatname, string email, DateOnly birthday)
        {
            ImplyTest((context, service) =>
            {
                ///Arrange
                RegistryRequest request = new RegistryRequest()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    Birthday = birthday,
                    Email = email
                };

                //Act
                var result = service.Register(request).Result;
                var resultMain = service.GetProfile(login);

                //Assert
                Assert.True(resultMain.Result.IsSuccess);

                //Clean
                if (result.IsSuccess)
                {
                    context.Users.Remove(result.Value);
                    context.SaveChanges();
                }
            });
        }
        public void GetProfile_GetExistingUserById_Value(string login, string password, string name, string surname, string fatname, string email, DateOnly birthday)
        {
            ImplyTest((context, service) =>
            {
                ///Arrange
                RegistryRequest request = new RegistryRequest()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    Birthday = birthday,
                    Email = email
                };

                //Act
                var result = service.Register(request).Result;
                var resultMain = service.GetProfile(result.Value.Id.Value);

                //Assert
                Assert.True(resultMain.Result.IsSuccess);

                //Clean
                if (result.IsSuccess)
                {
                    context.Users.Remove(result.Value);
                    context.SaveChanges();
                }
            });
        }
        public static TheoryData<string, string, string, string, string, string, DateOnly,  string> RegistryCases3 = new TheoryData<string, string, string, string, string, string, DateOnly, string>()
        {
            {"user1","password","Iname","surname","fatname","r.gabzalil@mail.ru",new DateOnly(2027,12,31),"user2" },
            {"user1","password","Iname","surname","fatname","",new DateOnly(2027,12,31),"user2" },
        };
        [Theory, MemberData(nameof(RegistryCases3))]
        public void ChangeProfile_ChangeExistingUserLogin_ErrorMessage(string login, string password, string name, string surname, string fatname, string email, DateOnly birthday,string newLogin)
        {
            ImplyTest((context, service) =>
            {
                ///Arrange
                RegistryRequest request = new RegistryRequest()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    Birthday = birthday,
                    Email = email
                };
                ChangeProfileRequest request2 = new ChangeProfileRequest()
                {
                    Login = newLogin,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    Birthday = birthday,
                    Email = email
                };


                //Act
                var result = service.Register(request).Result;
                var resultMain = service.ChangeProfile(request2,result.Value).Result;
                var changedProfile = service.GetProfile(request2.Login).Result;

                //Assert
                Assert.True(resultMain.IsSuccess);
                Assert.True(changedProfile.IsSuccess);
                Assert.Equal(resultMain.Value, changedProfile.Value);

                //Clean
                if (resultMain.IsSuccess)
                {
                    context.Remove(changedProfile.Value);
                    context.SaveChanges();
                }
            });
        }

        private void ImplyTest(Action<UserContext, UserService> test)
        {
            DbContextOptionsBuilder<UserContext> optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            DbContextOptions<UserContext> options = optionsBuilder.UseInMemoryDatabase("testDb").Options;
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider();
            UserContext context = new UserContext(options);
            IUserRepository repository = new UsersRepository(context);

            test(context, new UserService(repository,null));
        }
    }
}