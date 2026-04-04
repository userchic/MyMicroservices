using CSharpFunctionalExtensions;
using TextPostsService.DTO;
using TextPostsService.Model;

namespace TextPostsService.Abstractions
{
    public interface ITextPostService
    {
        Task<Result<TextPost, string>> CreatePost(CreatePostRequest request,int userId);
        Task<Result<TextPost, string>> UpdatePost(UpdatePostRequest request, int userId);
        Task<Result<string, string>> DeletePost(int id,int userId);
        Task<Result<TextPost, string>> GetPost(int id);
        Task<ICollection<TextPost>> GetUserPostsPage(int id, int userId);
    }
}
