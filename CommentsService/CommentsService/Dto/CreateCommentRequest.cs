namespace CommentsService.Dto
{
    public record CreateCommentRequest
    {
        public string Text { get; set; }
        public int PostId { get; set; }
    }
}
