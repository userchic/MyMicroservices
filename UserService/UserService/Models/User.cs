using AuthService.DTO;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class User
    {
        [Key]
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fatname { get; set; }
        public string Email { get; set; }
        public DateOnly Birthday { get; set; }
        public static User Create(RegistryRequest request)
        {
            return new User()
            {
                Login=request.Login,
                Name=request.Name,
                Surname=request.Surname,
                Fatname=request.Fatname,
                Birthday=request.Birthday,
                Email=request.Email,
                Password=request.Password
            };
        }
        public override bool Equals(object? obj)
        {
            if (obj is User)
            {
                User user = (User)obj;
                return user.Login==Login &&
                    user.Password==Password &&
                    user.Name==Name &&
                    user.Surname==Surname &&
                    user.Fatname==Fatname &&
                    user.Email==Email &&
                    user.Birthday==Birthday;
            }
            return false;
        }
    }
}
