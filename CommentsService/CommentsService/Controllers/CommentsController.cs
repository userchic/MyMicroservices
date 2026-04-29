using CommentsService.Abstractions;
using CommentsService.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.IdentityModel.Tokens.Jwt;

namespace CommentsService.Controllers
{
    public class CommentsController : Controller
    {
        ILogger? logger;
        ICommentService commentService;

        IValidator<CreateCommentRequest> createRequestValidator;
        IValidator<UpdateCommentRequest> updateRequestValidator;

        Counter getCommentPageCounter;
        Counter createCommentCounter;
        Counter updateCommentCounter;
        Counter deleteCommentCounter;
        public CommentsController(ICommentService commentService, IValidator<UpdateCommentRequest> updateRequestValidator,IValidator<CreateCommentRequest>  createRequestValidator,ILogger<CommentsController>? logger)
        {
            this.logger = logger;
            this.commentService= commentService;
            this.createRequestValidator = createRequestValidator;
            this.updateRequestValidator = updateRequestValidator;
            getCommentPageCounter = Metrics.CreateCounter("getCommentPageCounter", "Increments on getting comments page");
            createCommentCounter = Metrics.CreateCounter("createCommentCounter", "Increments on creating comment");
            updateCommentCounter = Metrics.CreateCounter("updateCommentCounter", "Increments on updating comment");
            deleteCommentCounter = Metrics.CreateCounter("deleteCommentCounter", "Increments on deleting comment");
        }
        /// <summary>
        /// Запрос на получение страницы комментариев под конкретным постом
        /// </summary>
        /// <param name="postId">Идентификатор поста</param>
        /// <param name="page">Нужная страница</param>
        /// <returns>Возвращает посты, либо error</returns>
        [HttpGet]
        public async Task<IActionResult> GetCommentsPage(int postId,int page)
        {
            getCommentPageCounter.Inc();
            getCommentPageCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            if (postId < 0)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор поста не может быть меньше 0" });
            }
            if (page <= 0)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор страницы не может быть меньше 1" });
            }
            var result = commentService.GetCommentsPage(postId, page);
            return Json(result);
        }
        /// <summary>
        /// Запрос на создание коментария под конкретным постом от своего имени.
        /// </summary>
        /// <param name="request">контракт о создании комментария</param>
        /// <returns>Возвращает новые комментарий, либо errors или error</returns>
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentRequest request)
        {
            createCommentCounter.Inc();
            createCommentCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            var validationResult = createRequestValidator.Validate(request);
            if (validationResult.IsValid)
            {
                return Json(await commentService.AddComment(request, userId.Value));
            }
            else
            {
                logger?.LogWarning("Ошибок валидации при создании комментария {Count}",validationResult.Errors.Count);
                return Json(new { errors = validationResult.Errors });
            }
        }
        /// <summary>
        /// Запрос на изменение конкретного комментария
        /// </summary>
        /// <param name="request">Контракт на изменение комментария</param>
        /// <returns>Возвращает message при успехе, либо errors или error</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentRequest request)
        {
            updateCommentCounter.Inc();
            updateCommentCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            var validationResult = updateRequestValidator.Validate(request);
            if (validationResult.IsValid)
            {
                var result = await commentService.UpdateComment(request, userId.Value);
                if (result.IsSuccess)
                {
                    return Json(new { message = result.Value });
                }
                else
                {
                    return Json(new { error = result.Error });
                }
            }
            else
            {
                logger?.LogWarning("Ошибок валидации при редактировании комментария {Count}",validationResult.Errors.Count);
                return Json(new { errors = validationResult.Errors });
            }
        }
        /// <summary>
        /// Запрос на удаление комментария
        /// </summary>
        /// <param name="commentId">Идентификатор комментария</param>
        /// <returns>Возвращает message в случае успеха, либо errors или error</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            deleteCommentCounter.Inc();
            deleteCommentCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            var result = await commentService.DeleteComment(userId.Value,commentId);
            if (result.IsSuccess)
            {
                return Json(new { message = result.Value });
            }
            else
            {
                return Json(new { error = result.Error });
            }
        }
        private int? GetUserId()
        {
            string token = HttpContext.Request.Headers.FirstOrDefault(header => header.Key.ToLower() == "myauth").Value;
            return GetUserIdFromToken(DecipherToken(token));
        }
        private JwtSecurityToken DecipherToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }
        private int? GetUserIdFromToken(JwtSecurityToken token)
        {
            try
            {
                return int.Parse(token.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value);
            }
            catch
            {
                logger?.LogError("Кажется получен некорректный userId или его нет");
                return null;
            }
        }
    }
}
