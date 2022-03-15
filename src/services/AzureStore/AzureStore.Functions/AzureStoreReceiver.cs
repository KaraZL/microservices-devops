using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureStore.Functions
{
    public static class AzureStoreReceiver
    {
        [FunctionName("AzureStoreReceiver")]
        public static void Run([ServiceBusTrigger("zafina-queue", Connection = "ServiceBusTrigger")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
