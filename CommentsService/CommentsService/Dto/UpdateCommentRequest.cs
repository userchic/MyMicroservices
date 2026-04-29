namespace CommentsService.Dto
{
    public record UpdateCommentRequest
    {
        public string NewText { get; set; }
        public int CommentId { get; set; }
    }
}
