namespace AuthService.DTO
{
    /// <summary>
    /// Контракт запроса на регистрацию
    /// </summary>
    public record RegistryRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string Fatname { get; set; }
        /// <summary>
        /// Емаил
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateOnly Birthday { get; set; }

    }
}
