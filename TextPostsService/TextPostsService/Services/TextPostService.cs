using CSharpFunctionalExtensions;
using TextPostsService.Abstractions;
using TextPostsService.DTO;
using TextPostsService.Model;

namespace TextPostsService.Services
{
    public class TextPostService : ITextPostService
    {
        ITextPostRepository postRep;
        ILogger? logger;
        public TextPostService(ITextPostRepository postRep,ILogger<TextPostService>? logger)
        {
            this.postRep = postRep;
            this.logger = logger;
        }

        public async Task<Result<TextPost, string>> CreatePost(CreatePostRequest request,int userId)
        {
            TextPost newPost = TextPost.Create(request,userId);
            newPost=await postRep.Create(newPost);
            await postRep.Save();
            return newPost;
        }
        public async Task<Result<TextPost, string>> UpdatePost(UpdatePostRequest request, int userId)
        {
            TextPost post = await postRep.GetUserPost(request.Id, userId);
            if (post is null)
            {
                logger?.LogWarning("Принят запрос на редактирование несуществующего поста {Id}", request.Id);
                return post.ToResult("Пост не найден или не принадлежит вам");
            }
            post.Text = request.Text;
            postRep.Update(post);
            await postRep.Save();
            return post;
        }
        public async Task<Result<string, string>> DeletePost(int id,int userId)
        {
            TextPost postToBeDeleted = await postRep.GetUserPost(id,userId);
            if (postToBeDeleted == null)
            {
                logger?.LogWarning("Принят запрос на удаление несуществующего поста {Id}", id);
                return ((string)null).ToResult("Пост не найден или не принадлежит вам");
            }
            postRep.Delete(postToBeDeleted);
            await postRep.Save();
            return "Успешно удален пост".ToResult("Ошибка? Но тут ведь невозможна ошибка!");
        }

        public async Task<Result<TextPost, string>> GetPost(int id)
        {
            TextPost post = await postRep.GetPost(id);
            if (post is null)
                logger?.LogWarning("Принят запрос на получение несуществующего поста {Id}", id);
            return post.ToResult("Не найден нужный пост");
        }

        public async Task<ICollection<TextPost>> GetUserPostsPage(int page, int userId)
        {
            return await postRep.GetUserPostsPage(page,userId);
        }

        
    }
}
