using System.Threading.Tasks;

namespace AzureStore.API.ServiceBus
{
    public interface IQueueService
    {
        Task SendMessageAsync(string str);
    }
}
