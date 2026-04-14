namespace NotificationsService.DTO
{
    public record Subscription
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int PosterId { get; set; }
    }
}
