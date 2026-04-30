using CommentsService.Abstractions;
using CommentsService.Dto;
using CommentsService.Models;
using CSharpFunctionalExtensions;

namespace CommentsService.Services
{
    public class CommentService : ICommentService
    {
        ICommentsRepository _commentsRepository;
        ILogger? logger;
        public CommentService (ICommentsRepository commentsRepository,ILogger<CommentService>? logger)
        {
            _commentsRepository = commentsRepository;
            this.logger = logger;
        }
        public async Task<Comment> AddComment(CreateCommentRequest request, int userId)
        {
            Comment newComment = new Comment() { CreationTime=DateTime.Now,PostId= request.PostId, Text= request.Text, OwnerUserId=userId};
            await _commentsRepository.CreateComment(newComment);
            await _commentsRepository.Save();
            return newComment;
        }
        public ICollection<Comment> GetCommentsPage(int postId, int page)
        {
            return _commentsRepository.GetCommentsPage(postId, page);
        }
        public Comment? GetComment(int commentId)
        {
            return _commentsRepository.GetComment(commentId);
        }
        public async Task<Result<string, string>> UpdateComment(UpdateCommentRequest request,int userId)
        {
            var existingComment = _commentsRepository.GetComment(request.CommentId);
            if (existingComment == null)
            {
                logger?.LogWarning("Запрос на редактирование несуществующего комментария");
                return ((string)null).ToResult("Не найден редактируемый комментарий");
            }
            if (existingComment.OwnerUserId != userId)
            {
                logger?.LogWarning("Запрос на редактирование чужого комментария");
                return ((string)null).ToResult("Вы не можете редактировать чужие комментарии");
            }
            existingComment.Text = request.NewText;
            _commentsRepository.UpdateComment(existingComment);
            await _commentsRepository.Save();
            return "Успешно изменен комментарий".ToResult("Тут всё должно быть нормально");
        }
        public async Task<Result<string, string>> DeleteComment(int userId, int commentId)
        {
            var existingComment = _commentsRepository.GetComment(commentId);
            if (existingComment == null)
            {
                logger?.LogWarning("Запрос на удаление несуществующего комментария");
                return ((string)null).ToResult("Не найден удаляемый комментарий");
            }
            if (existingComment.OwnerUserId != userId)
            {
                logger?.LogWarning("Запрос на удаление чужого комментария");
                return ((string)null).ToResult("Вы не можете удалять чужие комментарии");
            }
            _commentsRepository.DeleteComment(existingComment);
            await _commentsRepository.Save();
            return "Успешно удален комментарий".ToResult("Тут всё должно быть нормально");
        }


    }
}
