namespace TextPostsService.DTO
{
    public record UpdatePostRequest
    {
        public string Text { get; set; }
        public int Id { get; set; }
    }
}
