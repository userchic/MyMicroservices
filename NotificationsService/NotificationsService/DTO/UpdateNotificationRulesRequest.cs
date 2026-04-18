namespace NotificationsService.DTO
{
    public record UpdateNotificationRulesRequest
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
