using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderManagementCommon.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderManagementCommon.Workers;

public class EventConsumerWorker(IConfiguration configuration, IServiceProvider provider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:Hostname"]!,
            UserName = configuration["RabbitMQ:Username"]!,
            Password = configuration["RabbitMQ:Password"]!,
        };


        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var queueName = configuration["RabbitMQ:QueueName"]!;
        var exchangeName = configuration["RabbitMQ:ExchangeName"]!;


        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken
        );


        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );


        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: "#",
            cancellationToken: stoppingToken
        );


        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;


                await GestisciEvento(routingKey, messageJson, stoppingToken);


                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception)
            {
                await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };


        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );


        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task GestisciEvento(string routerKey, string messageJson, CancellationToken cancellationToken)
    {
        Type domainEventType = typeof(DomainEvent);
        var assembliesToScan = new[] { domainEventType.Assembly, Assembly.GetEntryAssembly() };

        Type? type = assembliesToScan
            .SelectMany(a => a!.GetTypes())
            .FirstOrDefault(t => t.Name == routerKey && domainEventType.IsAssignableFrom(t));


        if (type == null)
            return;

        DomainEvent? domainEvent = JsonSerializer.Deserialize(messageJson, type) as DomainEvent;
        if (domainEvent == null)
            return;

        var handlerType = typeof(IEventConsumer<>).MakeGenericType(domainEvent.GetType());
        var method = handlerType.GetMethod(nameof(IEventConsumer<DomainEvent>.Consume));
        if (method == null)
            return;

        using var scope = provider.CreateScope();

        var handlers = scope.ServiceProvider.GetServices(handlerType);

        if (handlers == null)
            return;

        foreach (var handler in handlers)
        {
            try
            {
                await (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
            }
            catch (Exception) { }
        }
    }
}