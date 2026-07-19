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

        //Dichiara l'exchange nel caso non sia gia creato
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken
        );

        //Dichiara la propria coda
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true, //per fare in modo che la coda sopravvia al riavvio del server
            exclusive: false, //permette alla coda di essere utilizzata da piu consumer
            autoDelete: false, //la coda non viene eliminata quando tutti i consumer vengono disconnessi
            arguments: null,
            cancellationToken: stoppingToken
        );


        //effettuo il binding della coda con l'exchange
        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: "#", //significa che sono interessato a tutti i messaggi
            cancellationToken: stoppingToken
        );


        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                //Per ogni messaggio nella coda
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                //gestisco il suo evento
                await GestisciEvento(routingKey, messageJson, stoppingToken);

                //Se ha successo mando un ACK alla coda per dirgli che e stato processato correttamente
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception)
            {
                //Dico che ce stato un errore e che dovro riprovare
                await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        //viene collegato il consumer
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