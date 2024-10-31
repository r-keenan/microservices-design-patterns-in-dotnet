using Azure.Messaging.ServiceBus;

WriteLine("Azure Service Bus Message Consumer");

string connectionString = "AzureServiceBusConnectionString";
string queueName = "QueueName";

await using var client = new ServiceBusClient(connectionString);
ServiceBusReceiver receiver = client.CreateReceiver(queueName);
ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

string body = receivedMessage.Body.ToString();
WriteLine(body);
