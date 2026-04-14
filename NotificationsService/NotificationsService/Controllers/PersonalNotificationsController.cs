using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotificationsService.Abstractions;
using NotificationsService.DTO;
using NotificationsService.Models;
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
        ILogger logger;

        public PersonalNotificationsController(IValidator<UpdateNotificationRulesRequest> updateRequestValidator,IValidator<CreateNotificationRulesRequest> createRequestValidator,IPersonalNotificationRulesService personalNotifService,ILogger<PersonalNotificationsController> logger)
        {
            this.logger = logger;
            personalNotificationService = personalNotifService;
            this.createRequestValidator = createRequestValidator;
            this.updateRequestValidator = updateRequestValidator;
        }
        [HttpGet]
        public async Task<IActionResult> GetPersonalNotificationsRule()
        {
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
        [HttpPost]
        public async Task<IActionResult> CreatePersonalNotificationsRules(CreateNotificationRulesRequest request)
        {
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
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalNotificationsRules(UpdateNotificationRulesRequest request)
        {
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
        [HttpDelete]
        public async Task<IActionResult> DeletePersonalNotificationsRules()
        {
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
