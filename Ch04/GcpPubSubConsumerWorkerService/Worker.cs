using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GcpPubSubConsumerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IGcpPubSubConsumer _pubSubConsumer;

        public Worker(IGcpPubSubConsumer pubSubConsumer, ILogger<Worker> logger)
        {
            _pubSubConsumer = pubSubConsumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _pubSubConsumer.Start();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _pubSubConsumer.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}
