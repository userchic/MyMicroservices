namespace NotificationsService.DTO
{
    public record CreateNotificationRulesRequest
    {
        public string NotificationService { get; set; }
        public string InternalServiceIdentificator { get; set; }
    }
}
