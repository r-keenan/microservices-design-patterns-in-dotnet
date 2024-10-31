using MassTransit;

WriteLine("RabbitMQ Message Consumer using MassTransit");

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint(
        "appointment-created-event",
        e =>
        {
            e.Consumer<AppointmentCreatedConsumer>();
        }
    );
});

await busControl.StartAsync(new CancellationToken());

try
{
    WriteLine("Press enter to exit");

    await Task.Run(() => ReadLine());
}
finally
{
    await busControl.StopAsync();
}
