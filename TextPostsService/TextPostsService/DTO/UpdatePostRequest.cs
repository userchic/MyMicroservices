namespace TextPostsService.DTO
{
    public record UpdatePostRequest
    {
        /// <summary>
        /// Измененный текст поста
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// идентификатор редактируемого поста
        /// </summary>
        public int Id { get; set; }
    }
}
