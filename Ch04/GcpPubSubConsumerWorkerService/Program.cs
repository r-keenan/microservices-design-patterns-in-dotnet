using GcpPubSubConsumerWorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddScoped<IGcpPubSubConsumer, GcpPubSubConsumer>();
    })
    .Build();

await host.RunAsync();
