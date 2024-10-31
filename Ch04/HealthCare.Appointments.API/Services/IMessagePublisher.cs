namespace HealthCare.Appointments.API.Services
{
    public interface IMessagePublisher
    {
        Task PublishMessage<T>(T data, string topicName);
    }
}
