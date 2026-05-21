using FluentValidation;
using MessageService.Abstractions;
using MessageService.DTO;
using MessageService.Models;
using MessageService.Services;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class MessageController:Controller
    {
        IMessageService messageService;
        ILogger logger;
        IValidator<CreateMessageRequest> createRequestValidator;
        IValidator<UpdateMessageRequest> updateRequestValidator;
        Counter getMessagesPageCounter;
        Counter sendMessageCounter;
        Counter updateMessageCounter;
        Counter deleteMessageCounter;
        public MessageController(ILogger<MessageController> logger, IMessageService messageService)
        {
            this.messageService = messageService;
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult GetMessagesPageFromDialog(int dialogId,int page)
        {
            getMessagesPageCounter.Inc();
            getMessagesPageCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            if (dialogId <= 0)
            {
                logger?.LogWarning("Получен запрос на получение страницы сообщений диалога с неправильным идентификатором диалога");
                return Json(new { error = "Неправильный номер диалога, он должен быть больше 0." });
            }
            if (page <= 0)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор страницы не может быть меньше 1." });
            }
            return Json(messageService.GetMessagesPageFromDialog(dialogId, userId.Value, page));
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageRequest request)
        {
            sendMessageCounter.Inc();
            sendMessageCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            var validationResult = createRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Json(new { errors = validationResult.Errors });
            }
            Message newMessage = await messageService.CreateMessage(request, userId.Value);
            return Json(newMessage);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMessage(UpdateMessageRequest request)
        {
            updateMessageCounter.Inc();
            updateMessageCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            var validationResult = updateRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Json(new { errors = validationResult.Errors });
            }
            var result = await messageService.UpdateMessage(request, userId.Value);
            if (result.IsSuccess)
            {
                return Json(result.Value);
            }
            else
                return Json(result.Error);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            deleteMessageCounter.Inc();
            deleteMessageCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            var result = await messageService.DeleteMessage(messageId, userId.Value);
            if (result.IsSuccess)
            {
                return Json(result.Value);
            }
            else
                return Json(result.Error);
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
