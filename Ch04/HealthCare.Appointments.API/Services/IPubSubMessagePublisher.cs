namespace HealthCare.Appointments.API.Services
{
    public interface IPubSubMessagePublisher
    {
        Task PublishMessage<T>(T data, string topicName);
    }
}
