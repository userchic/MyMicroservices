using AuthService.DTO;
using AuthService.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestProject
{
    public class ValidationTesting
    {
        IValidator<ChangeProfileRequest> changeProfileRequestValidator = new ChangeProfileRequestValidator(); 
        IValidator<LoginRequest> loginRequestValidator = new LoginRequestValidator();
        IValidator<RegistryRequest> registryRequestValidator = new RegistryRequestValidator();


        public static TheoryData<string, string, string, string, string, string, DateOnly,int> changeProfileCases = new TheoryData<string, string, string, string, string, string, DateOnly,int>()
        {
            {"user1","password","Iname","surname","fatname","r.gabzalil@mail.ru",new DateOnly(2027,12,31),1 },
            {"user","password","Iname","surname","fatname","r.gabzalil@mail.ru",new DateOnly(2000,12,31),1 },
            {"user1","pass","Iname","surname","fatname","r.gabzalil@mail.ru",new DateOnly(2000,12,31),1 },
            {"user1","password","Iname","surname","fatname","r.gabzalilmail.ru",new DateOnly(2000,12,31),1 },
            {"user1","password","nm","surname","fatname","r.gabzalilmail.ru",new DateOnly(2000,12,31),2 },
            {"user1","password","nm","surname","fatname","",new DateOnly(2000,12,31),1 },
        };
        [Theory,MemberData(nameof(changeProfileCases))]
        public void ChangeProfileValidationTest(string login, string password,string name,string surname,string fatname,string email,DateOnly birthday,int amountOfErrors)
        {
            ChangeProfileRequest request = new ChangeProfileRequest()
            {
                Login = login,
                Password = password,
                Name = name,
                Surname = surname,
                Fatname = fatname,
                Birthday = birthday,
                Email = email
            };
            var result = changeProfileRequestValidator.Validate(request);
            Assert.Equal(amountOfErrors, result.Errors.Count);
        }
        [Theory,MemberData(nameof(changeProfileCases))]
        public void RegisterValidationTest(string login, string password, string name, string surname, string fatname, string email, DateOnly birthday, int amountOfErrors)
        {
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
            var result = registryRequestValidator.Validate(request);
            Assert.Equal(amountOfErrors, result.Errors.Count);
        }
        [Theory]
        [InlineData(" "," ",2)]
        [InlineData("", " ",2)]
        [InlineData(" ", "",2)]
        [InlineData("", "",2)]
        [InlineData("","a",1)]
        [InlineData("a","",1)]
        [InlineData("a","a",0)]
        [InlineData("login","password",0)]
        public void LoginValidationTest(string login,string password,int amountOfErrors)
        {
            LoginRequest request = new LoginRequest()
            {
                Login = login,
                Password = password
            };
            var result = loginRequestValidator.Validate(request);
            Assert.Equal(amountOfErrors, result.Errors.Count);
        }
    }
}
