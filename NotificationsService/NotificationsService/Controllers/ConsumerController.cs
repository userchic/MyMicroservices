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
