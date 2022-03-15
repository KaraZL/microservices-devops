using AzureStore.API.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzureStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceBusController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public ServiceBusController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageToAzureBus(string message)
        {
            await _queueService.SendMessageAsync(message);

            return Ok(message);
        }
    }
}
