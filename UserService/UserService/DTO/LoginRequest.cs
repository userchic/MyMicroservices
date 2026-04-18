using System.ComponentModel.DataAnnotations;

namespace AuthService.DTO
{
    /// <summary>
    /// Контракт для запроса на вход в систему
    /// </summary>
    public record LoginRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
