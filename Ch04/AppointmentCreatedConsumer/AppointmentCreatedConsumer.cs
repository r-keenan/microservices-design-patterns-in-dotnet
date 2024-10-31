using HealthCare.SharedAssets.Messages;
using MassTransit;
using Newtonsoft.Json;

public class AppointmentCreatedConsumer : IConsumer<AppointmentMessage>
{
    public async Task Consume(ConsumeContext<AppointmentMessage> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        WriteLine($"ApoointmentCreated message: {jsonMessage}");
    }
}
