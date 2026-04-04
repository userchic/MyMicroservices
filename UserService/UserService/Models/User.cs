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
        public DateTime Birthday { get; set; }
        public static User Create(RegistryRequest request)
        {
            return new User()
            {
                Login=request.Login,
                Name=request.Name,
                Surname=request.Surname,
                Fatname=request.Fatname,
                Birthday=request.Birthday,
                Password=request.Password
            };
        }
    }
}
