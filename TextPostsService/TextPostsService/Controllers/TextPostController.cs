using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TextPostsService.Abstractions;
using TextPostsService.DTO;
using TextPostsService.Producer;

namespace TextPostsService.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class TextPostController : Controller
    {
        ITextPostService postService;
        IPostProducedMessagerService producer;
        IValidator<CreatePostRequest> createPostValidator;
        IValidator<UpdatePostRequest> updatePostValidator;
        ILogger<TextPostController> logger;
        public TextPostController(ILogger<TextPostController> logger,ITextPostService postService, IValidator<UpdatePostRequest> updatePostValidator,IValidator<CreatePostRequest> createPostValidator, IPostProducedMessagerService producer)
        {
            this.postService = postService;
            this.updatePostValidator = updatePostValidator;
            this.createPostValidator = createPostValidator;
            this.logger = logger;
            this.producer = producer;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostRequest request)
        {
            var validationResult = createPostValidator.Validate(request);
            if (validationResult.IsValid)
            {
                var userId = GetUserId();

                if (userId.HasValue)
                {
                    var result = await postService.CreatePost(request, userId.Value);
                    if (result.IsSuccess)
                    {
                        producer.SendPostCreatedMessageAsync("postCreated", userId.Value);
                        return Json(result.Value);
                    }
                    else
                        return Json(new { error = result.Error });
                }
                else
                    return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            return Json(new { errors = validationResult.Errors });
        }
        [HttpDelete()]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (id>-1)
            {
                var userId = GetUserId();
                if (userId.HasValue)
                {
                    var result = await postService.DeletePost(id, userId.Value);
                    if (result.IsSuccess)
                        return Json(result.Value);
                    else
                        return Json(new { error = result.Error });
                }
                else
                    return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            return Json(new { error = "Указан некорректный идентификатор удаляемого поста." });
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostRequest request)
        {
            var validationResult = updatePostValidator.Validate(request);
            if (validationResult.IsValid)
            {
                var userId = GetUserId();

                if (userId.HasValue)
                {
                    var result = await postService.UpdatePost(request, userId.Value);
                    if (result.IsSuccess)
                        return Json(result.Value);
                    else
                        return Json(new { error = result.Error });
                }
                else
                    return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            return Json(new { errors = validationResult.Errors });
        }
        [HttpGet]
        public async Task<IActionResult> GetPost(int id)
        {
            if (id > -1)
            {
                var result = await postService.GetPost(id);
                if (result.IsSuccess)
                    return Json(result.Value);
                else
                    return Json(new { error = result.Error });
            }
            return Json(new { error = "Указан некорректный идентификатор поста." });
        }
        [HttpGet]
        public async Task<IActionResult> GetUserPostsPage(int page, int userId)
        {
            if (page <= -1)
            {
                return Json(new { error = "Некорректный номер страницы" });
            }
            if(userId <= -1)
            {
                return Json(new { error = "Некорректный идентификатор пользователя" });
            }
            var result = await postService.GetUserPostsPage(page, userId);
            return Json(result);
        }
        private int? GetUserId()
        {
            string token = HttpContext.Request.Headers.FirstOrDefault(header => header.Key == "MyAuth").Value;
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
                logger.LogError("Кажется получен некорректный userId или его нет");
                return null;
            }
        }
    }
}
