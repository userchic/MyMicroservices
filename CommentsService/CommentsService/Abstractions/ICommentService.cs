using CommentsService.Dto;
using CommentsService.Models;
using CSharpFunctionalExtensions;

namespace CommentsService.Abstractions
{
    public interface ICommentService
    {
        ICollection<Comment> GetCommentsPage(int postId, int page);
        Task<Comment> AddComment(CreateCommentRequest request, int userId);
        Task<Result<string,string>> UpdateComment(UpdateCommentRequest request, int userId  );
        Task<Result<string, string>> DeleteComment( int userId, int commentId);
    }
}
