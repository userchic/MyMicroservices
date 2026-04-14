namespace NotificationsService.DTO
{
    public record UpdateNotificationRulesRequest
    {
        public string NotificationService { get; set; }
        public string InternalServiceIdentificator { get; set; }
    }
}
