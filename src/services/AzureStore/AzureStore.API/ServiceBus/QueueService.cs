using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace AzureStore.API.ServiceBus
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;

        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string str)
        {
            // Because ServiceBusClient implements IAsyncDisposable, we'll create it 
            // with "await using" so that it is automatically disposed for us.
            await using var queueClient = new ServiceBusClient(_configuration.GetConnectionString("ServiceBusUrl"));
            var queueName = _configuration.GetConnectionString("QueueName");

            // The sender is responsible for publishing messages to the queue.
            ServiceBusSender sender = queueClient.CreateSender(queueName);
            ServiceBusMessage message = new ServiceBusMessage(str);
            await sender.SendMessageAsync(message);
        }
    }
}
