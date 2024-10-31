using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace AzureServiceBusConsumerWorkerService
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly ServiceBusProcessor _appointmentProcessor;
        private readonly string _appointmentSubscription;
        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            _appointmentSubscription = _configuration.GetValue<string>(
                "AppointmentProcessSubscription"
            );
            var client = new ServiceBusClient(_appointmentSubscription);

            _appointmentProcessor = client.CreateProcessor(
                "appointments",
                _appointmentSubscription
            );
        }

        public async Task Start()
        {
            _appointmentProcessor.ProcessMessageAsync += ProcessAppointment;
            _appointmentProcessor.ProcessErrorAsync += ErrorHandler;
            await _appointmentProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _appointmentProcessor.StopProcessingAsync();
            await _appointmentProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ProcessAppointment(ProcessMessageEventArgs args)
        {
            await args.CompleteMessageAsync(args.Message);
        }
    }
}
