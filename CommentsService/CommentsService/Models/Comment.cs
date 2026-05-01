namespace CommentsService.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int OwnerUserId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
