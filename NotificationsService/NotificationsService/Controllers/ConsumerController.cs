using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using NotificationsService.Consumer;

namespace NotificationsService.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class ConsumerController:Controller
    {
        ConsumerService consumerService;
        public ConsumerController(ConsumerService consumerService)
        {
            this.consumerService = consumerService;
        }
        /// <summary>
        /// Запрос на запуск консюмера читающего сообщения об опубликованных постах
        /// </summary>
        /// <param name="partition">Партиция из которой будет идти потребление</param>
        /// <returns>Возвращает message, либо error</returns>
        /// <remarks>Консюмер возможен только один и попытки создать еще не приведут ни к чему</remarks>
        [HttpPost]
        public IActionResult StartConsuming(int partition)
        {
            var result = consumerService.StartConsuming(partition);
            if (result.IsSuccess)
            {
                return Json(new { message = result.Value });
            }
            return Json(new {error=result.Error});
        }
        /// <summary>
        /// Запрос на остановку работы консюмера
        /// </summary>
        /// <returns>Возвращает message, либо error</returns>
        /// <remarks>Консюмер возможен только один и попытки создать еще не приведут ни к чему. Консюмер выключится сразу, либо где-то за 10 секунд если он не может найти сообщения.</remarks>
        [HttpPost]
        public IActionResult StopConsuming()
        {
            var result = consumerService.StopConsuming();
            if (result.IsSuccess)
            {
                return Json(new { message = result.Value });
            }
            return Json(new { error = result.Error });
        }
    }
}
