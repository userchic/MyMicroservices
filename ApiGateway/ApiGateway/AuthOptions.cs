using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthService
{
    public class AuthOptions
    {
        public const string ISSUER = "UserService"; // издатель токена
        public const string AUDIENCE = "Users"; // потребитель токена
        const string KEY = "hujrfjhyigslriuejituoyradpgvegde";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
