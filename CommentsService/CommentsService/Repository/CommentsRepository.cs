using CommentsService.Abstractions;
using CommentsService.DataBase;
using CommentsService.Models;

namespace CommentsService.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        CommentsContext context;
        public CommentsRepository(CommentsContext context)
        {
            this.context = context;
        }
        public async Task<Comment> CreateComment(Comment newComment)
        {
            await context.Comments.AddAsync(newComment);
            return newComment;
        }
        public Comment? GetComment(int commentId)
        {
            return context.Comments.FirstOrDefault((comment) => comment.Id == commentId);
        }
        public ICollection<Comment> GetCommentsPage(int postId, int page)
        {
            return context.Comments.Where((comment) => comment.PostId == postId).Skip((page-1)*10).Take(10).ToArray();
        }
        public void UpdateComment(Comment updatedComment)
        {
            context.Comments.Update(updatedComment);
        }
        public void DeleteComment(Comment commentToBeDeleted)
        {
            context.Comments.Remove(commentToBeDeleted);
        }
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }


    }
}
