namespace TextPostsService.DTO
{
    public record CreatePostRequest
    {
        /// <summary>
        /// Текст поста
        /// </summary>
        public string Text { get; set; }
    }
}
