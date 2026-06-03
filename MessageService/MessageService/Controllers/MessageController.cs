using FluentValidation;
using MessageService.Abstractions;
using MessageService.DTO;
using MessageService.Models;
using MessageService.Services;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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
        Counter getDialogCounter;
        Counter getMessagesPageCounter;
        Counter sendMessageCounter;
        Counter updateMessageCounter;
        Counter deleteMessageCounter;
        public MessageController(ILogger<MessageController> logger, IMessageService messageService,IValidator<CreateMessageRequest> createRequestValidator,IValidator<UpdateMessageRequest>updateRequestValidator)
        {
            this.messageService = messageService;
            this.logger = logger;
            this.createRequestValidator = createRequestValidator;
            this.updateRequestValidator = updateRequestValidator;
            getDialogCounter = Metrics.CreateCounter("getDialogCounter" , "increments on getting dialog");
            getMessagesPageCounter = Metrics.CreateCounter("getMessagesPageCounter","increments on getting messages page");
            sendMessageCounter = Metrics.CreateCounter("sendMessageCounter","increments on sending message");
            updateMessageCounter = Metrics.CreateCounter("updateMessageCounter","increments on updating message");
            deleteMessageCounter = Metrics.CreateCounter("deleteMessageCounter", "increments on deleting message");
        }
        /// <summary>
        /// Запрос на получение страницы диалогов пользователя
        /// </summary>
        /// <param name="page"> Страница</param>
        /// <returns>Возвращает страницу с диалогами, либо error.</returns>
        [HttpGet]
        public IActionResult GetDialogsPage(int page)
        {
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            return Json(messageService.GetDialogsPage(userId.Value, page));
        }
        /// <summary>
        /// Запрос на получение даилога текущего пользователя с конкретным пользователем
        /// </summary>
        /// <param name="targetUserId">Идентификатор целевого пользователя</param>
        /// <returns>Диалог с указанным пользователем, либо error.</returns>
        [HttpGet]
        public IActionResult GetDialog(int targetUserId)
        {
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                logger?.LogWarning("Не распознан Id пользователя {userId}", userId.Value);
                return Json(new { error = "Идентификатор пользователя не распознан" });
            }
            return Json(messageService.GetDialog(targetUserId, userId.Value));
        }
        /// <summary>
        /// Запрос на получение страницы сообщений пользователя из диалога
        /// </summary>
        /// <param name="dialogId">Идентификатор диалога</param>
        /// <param name="page">Страница</param>
        /// <returns>Страницу с сообщениями из конкретного диалога, либо error.</returns>
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
                logger?.LogWarning("Недопустимый номер страницы {page}", page);
                return Json(new { error = "Идентификатор страницы не может быть меньше 1." });
            }
            return Json(messageService.GetMessagesPageFromDialog(dialogId, userId.Value, page));
        }
        /// <summary>
        /// Запрос на создание сообщения в рамках диалога, если диалог еще не нечат то он будет создан
        /// </summary>
        /// <param name="request">Запрос на создание сообщения, либо error или errors.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Запрос на изменение сообщения
        /// </summary>
        /// <param name="request">Запрос на изменение сообщения</param>
        /// <returns>сообщение об успехе, либо error или errors.</returns>
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
                return Json(new { error = result.Error });
        }
        /// <summary>
        /// Запрос на удаление сообщения
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения</param>
        /// <returns>сообщение об успехе, либо error</returns>
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
                return Json(new { error = result.Error });

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
