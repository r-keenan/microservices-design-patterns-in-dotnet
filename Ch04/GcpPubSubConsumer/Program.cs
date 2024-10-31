using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// This is mocked and will fail without a real ProjectId and SubscriptionId in the appsettings.json
var projectId = configuration["GoogleCloud:ProjectId"];
var subscriptionId = configuration["GoogleCloud:SubscriptionId"];

WriteLine($"Starting GCP PubSub Consumer for subscription: {subscriptionId}");

var subscriptionName = new SubscriptionName(projectId, subscriptionId);
var subscriber = await SubscriberClient.CreateAsync(subscriptionName);

// Set up cancellation token
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, args) =>
{
    args.Cancel = true;
    cts.Cancel();
};

try
{
    await subscriber.StartAsync(
        async (PubsubMessage message, CancellationToken cancelToken) =>
        {
            try
            {
                string text = message.Data.ToStringUtf8();
                WriteLine($"Message received: {text}");

                // Process your message here
                await ProcessMessageAsync(text);

                return SubscriberClient.Reply.Ack;
            }
            catch (Exception ex)
            {
                WriteLine($"Error processing message: {ex.Message}");
                // Nack the message if processing failed
                return SubscriberClient.Reply.Nack;
            }
        }
    );

    // Keep the application running until cancellation is requested
    await Task.Delay(Timeout.Infinite, cts.Token);
}
catch (OperationCanceledException)
{
    WriteLine("Subscription cancelled.");
}
catch (Exception ex)
{
    WriteLine($"Error in subscription: {ex.Message}");
}
finally
{
    // Cleanup
    await subscriber.StopAsync(TimeSpan.FromSeconds(15));
    WriteLine("Subscriber stopped.");
}

async Task ProcessMessageAsync(string messageContent)
{
    WriteLine($"Processing message: {messageContent}");
    WriteLine("Updating the db if needed");
    // Add your message processing logic here
    await Task.Delay(100); // Simulated processing
}
