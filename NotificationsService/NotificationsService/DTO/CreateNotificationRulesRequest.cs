namespace NotificationsService.DTO
{
    public record CreateNotificationRulesRequest
    {
        /// <summary>
        /// Сервис уведомлений
        /// </summary>
        public string NotificationService { get; set; }
        /// <summary>
        /// идентификатор пользователя в рамках сервиса
        /// </summary>
        public string InternalServiceIdentificator { get; set; }
    }
}
