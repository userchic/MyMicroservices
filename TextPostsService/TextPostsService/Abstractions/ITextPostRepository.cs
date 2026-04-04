using TextPostsService.Model;

namespace TextPostsService.Abstractions
{
    public interface ITextPostRepository:IRepository
    {
        Task<ICollection<TextPost>> GetUserPostsPage(int page, int userId);
        Task<TextPost?> GetPost(int id);
        Task<TextPost?> GetUserPost(int id,int userId);
        Task<TextPost> Create(TextPost post);
        void Update(TextPost post);
        void Delete(TextPost post);
    }
}
