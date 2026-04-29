using CommentsService.Models;

namespace CommentsService.Abstractions
{
    public interface ICommentsRepository:IRepository
    {
        Comment? GetComment(int commentId);
        ICollection<Comment> GetCommentsPage(int postId, int page);
        Task<Comment> CreateComment(Comment newComment);
        void UpdateComment(Comment updatedComment);
        void DeleteComment(Comment commentToBeDeleted);
    }
}
