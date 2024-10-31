using System.Text.Json;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;

namespace HealthCare.Appointments.API.Services
{
    public class PubSubMessagePublisher : IMessagePublisher
    {
        private readonly IConfiguration _configuration;

        public PubSubMessagePublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishMessage<T>(T data, string topicName)
        {
            var projectId = _configuration["GoogleCloud:ProjectId"];
            var topicId = _configuration["GoogleCloud:Topics:" + topicName];

            var newTopicName = new TopicName(projectId, topicId);

            // Create the publisher client
            await using var publisher = await PublisherClient.CreateAsync(newTopicName);

            // Serialize the message
            var messageJson = JsonSerializer.Serialize(data);
            var pubsubMessage = new PubsubMessage { Data = ByteString.CopyFromUtf8(messageJson) };

            // Publish the message
            var messagedId = await publisher.PublishAsync(pubsubMessage);
        }
    }
}
