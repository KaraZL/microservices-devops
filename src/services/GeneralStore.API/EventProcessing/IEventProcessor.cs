namespace GeneralStore.API.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
