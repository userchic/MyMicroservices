namespace MessageService.DTO
{
    public class CreateMessageRequest
    {
        public int RecieverId { get; set; }
        public string Text { get; set; }
    }
}
