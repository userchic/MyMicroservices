using Microsoft.AspNetCore.Mvc;
using SubscriptionsService.Abstractions;
using System.Buffers;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Pipelines;
using System.Text;

namespace SubscriptionsService.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class SubscriptionsController:Controller
    {
        ISubscriptionsService _subsService;
        ILogger logger;
        public SubscriptionsController (ISubscriptionsService subsService,ILogger<SubscriptionsController> logger)
        {
            _subsService = subsService;
            logger= logger;
        }
        [HttpGet]
        public IActionResult GetSubscriptions(int userId)
        {
            var subscriptions = _subsService.GetSubscriptions(userId);
            return Json(subscriptions);
        }
        [HttpGet]
        public IActionResult GetSubscribers(int userId)
        {
            return Json((_subsService.GetSubscribers(userId)).ToArray());
        }
        [HttpGet]
        public async Task<IActionResult> GetIsSubscribed(int targetUserId)
        {
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            return Json(new {
                isSubscribed = (await _subsService.IsSubscribed(userId.Value, targetUserId)).ToString() 
            });
        }
        [HttpPost]
        public async Task<IActionResult> Subscribe(int targetId)
        {
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            var result = await _subsService.Subscribe(userId.Value, targetId);
            if (result.IsSuccess)
            {
                return Json(new { message = result.Value });
            }
            return Json(new { error = result.Error });
        }
        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int targetId)
        {
            int? userId = GetUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Невозможно прочитать userId, некорректно прочитан/предоставлен токен" });
            }
            var result = await _subsService.UnSubscribe(userId.Value, targetId);
            if (result.IsSuccess)
            {
                return Json(new { message = result.Value });
            }
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
