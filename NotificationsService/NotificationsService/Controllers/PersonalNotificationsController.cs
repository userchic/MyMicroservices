using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotificationsService.Abstractions;
using NotificationsService.DTO;
using NotificationsService.Models;
using Prometheus;
using System.IdentityModel.Tokens.Jwt;

namespace NotificationsService.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class PersonalNotificationsController:Controller
    {
        IPersonalNotificationRulesService personalNotificationService;
        IValidator<CreateNotificationRulesRequest> createRequestValidator;
        IValidator<UpdateNotificationRulesRequest> updateRequestValidator;
        Counter createRuleCounter;
        Counter updateRuleCounter;
        Counter deleteRuleCounter;
        Counter getRuleCounter;

        ILogger logger;

        public PersonalNotificationsController(IValidator<UpdateNotificationRulesRequest> updateRequestValidator,IValidator<CreateNotificationRulesRequest> createRequestValidator,IPersonalNotificationRulesService personalNotifService,ILogger<PersonalNotificationsController> logger)
        {
            this.logger = logger;
            personalNotificationService = personalNotifService;
            this.createRequestValidator = createRequestValidator;
            this.updateRequestValidator = updateRequestValidator;
            createRuleCounter = Metrics.CreateCounter("createRuleCounter","increments on creating rule");
            updateRuleCounter = Metrics.CreateCounter("updateRuleCounter", "increments on updating rule");
            deleteRuleCounter = Metrics.CreateCounter("deleteRuleCounter", "increments on deleting rule");
            getRuleCounter = Metrics.CreateCounter("getRuleCounter", "increments on getting rule");
        }
        /// <summary>
        /// Запрос правила уведомления пользователя
        /// </summary>
        /// <returns>Возвращает правило, либо error</returns>
        /// <remarks>Пользователь определяется по payload JWT токена.
        /// Каждый пользователь может иметь только одно правило, это проверяется в реализации запросов.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetPersonalNotificationsRule()
        {
            getRuleCounter.Inc();
            getRuleCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            var result = await personalNotificationService.GetPersonalRule(userId.Value);
            if (result is not null)
                return Json(result);
            else
                return Json(new { error = "Правило уведомления не было найдено, скорее всего оно не существует" });
        }
        /// <summary>
        /// Запрос на создание правила уведомления пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Возвращает message, либо error</returns>
        /// <remarks>Пользователь определяется по payload JWT токена.
        /// Каждый пользователь может иметь только одно правило, это проверяется в реализации запросов.
        /// </remarks>

        [HttpPost]
        public async Task<IActionResult> CreatePersonalNotificationsRules(CreateNotificationRulesRequest request)
        {
            createRuleCounter.Inc();
            createRuleCounter.Publish();
            var validationRes = createRequestValidator.Validate(request);
            if (!validationRes.IsValid)
                return Json(new { errors = validationRes.Errors });
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            PersonalNotificationRule newRule = PersonalNotificationRule.Create(request);
            var result = await personalNotificationService.CreatePersonalRule(request,userId.Value);
            if (result.IsSuccess)
                return Json(new { message = result.Value });
            else
                return Json(new { error = result.Error });
        }
        /// <summary>
        /// Запрос на изменение правила уведомления пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Возвращает message, либо error</returns>
        /// <remarks>Пользователь определяется по payload JWT токена.
        /// Каждый пользователь может иметь только одно правило, это проверяется в реализации запросов.
        /// </remarks>
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalNotificationsRules(UpdateNotificationRulesRequest request)
        {
            updateRuleCounter.Inc();
            updateRuleCounter.Publish();
            var validationRes = updateRequestValidator.Validate(request);
            if (!validationRes.IsValid)
                return Json(new { errors = validationRes.Errors });
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            PersonalNotificationRule newRule = PersonalNotificationRule.Create(request);
            var result = await personalNotificationService.UpdatePersonalRule(request, userId.Value);
            if (result.IsSuccess)
                return Json(new { message = result.Value });
            else
                return Json(new { error = result.Error });
        }
        /// <summary>
        /// Запрос на удаление правила уведомления пользователя
        /// </summary>
        /// <returns>Возвращает message, либо error</returns>
        /// <remarks>Пользователь определяется по payload JWT токена.
        /// Каждый пользователь может иметь только одно правило, это проверяется в реализации запросов.
        /// </remarks>
        [HttpDelete]
        public async Task<IActionResult> DeletePersonalNotificationsRules()
        {
            deleteRuleCounter.Inc();
            deleteRuleCounter.Publish();
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            var result = await personalNotificationService.DeletePersonalRule( userId.Value);
            if (result.IsSuccess)
                return Json(new { message = result.Value });
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
                logger.LogError("Кажется получен некорректный userId или его нет");
                return null;
            }
        }
    }
}
