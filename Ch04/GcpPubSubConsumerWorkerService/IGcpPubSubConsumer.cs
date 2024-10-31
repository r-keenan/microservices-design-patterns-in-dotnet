namespace GcpPubSubConsumerWorkerService
{
    public interface IGcpPubSubConsumer
    {
        Task Start();
        Task Stop();
    }
}
