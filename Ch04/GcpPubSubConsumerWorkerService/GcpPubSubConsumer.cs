using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using static Google.Cloud.PubSub.V1.SubscriberClient;

namespace GcpPubSubConsumerWorkerService
{
    public class GcpPubSubConsumer : IGcpPubSubConsumer
    {
        private readonly SubscriptionName _subscriptionName;
        private readonly SubscriberClient _subscriber;
        private readonly ILogger<GcpPubSubConsumer> _logger;
        private Task _processingTask;
        private CancellationTokenSource _cancellationTokenSource;

        public GcpPubSubConsumer(
            string projectId,
            string subscriptionId,
            ILogger<GcpPubSubConsumer> logger
        )
        {
            _subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);
            _subscriber = SubscriberClient.Create(_subscriptionName);
            _logger = logger;
        }

        public Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _processingTask = _subscriber.StartAsync(HandleMessageAsync);
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
            _cancellationTokenSource?.Cancel();
            if (_processingTask != null)
            {
                await _subscriber.StopAsync(CancellationToken.None);
                await _processingTask;
            }
        }

        private async Task<Reply> HandleMessageAsync(
            PubsubMessage message,
            CancellationToken cancellationToken
        )
        {
            try
            {
                string messageData = message.Data.ToStringUtf8();
                _logger.LogInformation("Received message: {MessageData}", messageData);

                // Process your message here

                return Reply.Ack;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                return Reply.Nack; // Message will be redelivered
            }
        }
    }
}
