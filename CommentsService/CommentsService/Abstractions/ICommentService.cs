using CommentsService.Dto;
using CommentsService.Models;
using CSharpFunctionalExtensions;

namespace CommentsService.Abstractions
{
    public interface ICommentService
    {
        Comment? GetComment(int commentId);
        ICollection<Comment> GetCommentsPage(int postId, int page);
        Task<Comment> AddComment(CreateCommentRequest request, int userId);
        Task<Result<string,string>> UpdateComment(UpdateCommentRequest request, int userId  );
        Task<Result<string, string>> DeleteComment( int userId, int commentId);
    }
}
