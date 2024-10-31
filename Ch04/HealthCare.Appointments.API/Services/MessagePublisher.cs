using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace HealthCare.Appointments.API.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConfiguration _configuration;

        public MessagePublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishMessage<T>(T data, string topicName)
        {
            await using var client = new ServiceBusClient(
                _configuration["AzureServiceBusConnection"]
            );

            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(data);
            ServiceBusMessage finalMessage = new ServiceBusMessage(
                Encoding.UTF8.GetBytes(jsonMessage)
            )
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);

            await client.DisposeAsync();
        }
    }
}
