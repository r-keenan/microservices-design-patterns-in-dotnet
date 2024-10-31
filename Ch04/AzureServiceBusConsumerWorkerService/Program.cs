using AzureServiceBusConsumerWorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddScoped<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
    })
    .Build();

await host.RunAsync();
