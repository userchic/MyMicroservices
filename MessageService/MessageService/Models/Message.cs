using MessageService.DTO;

namespace MessageService.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int DialogId { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        //public Dialog Dialog { get; set; }
    }
}
