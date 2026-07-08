using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace OrderManagementCommon.Workers;

public class EventPublisherWorker(IConfiguration configuration, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:Hostname"]!,
            UserName = configuration["RabbitMQ:Username"]!,
            Password = configuration["RabbitMQ:Password"]!
        };

        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            exchange: configuration["RabbitMQ:ExchangeName"]!,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken
        );

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishPendingEventsAsync(channel, stoppingToken);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }

    private async Task PublishPendingEventsAsync(IChannel channel, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderManagementDbContext>();

        var eventiPendenti = dbContext.Eventi
            .Where((e) => !e.IsCompletato)
            .OrderBy((e) => e.MomentoCreazione)
            .Take(20)
            .ToList();

        foreach (var evento in eventiPendenti)
        {
            await channel.BasicPublishAsync(
                exchange: configuration["RabbitMQ:ExchangeName"]!,
                routingKey: evento.Tipo,
                mandatory: true,
                basicProperties: new BasicProperties()
                {
                    Persistent = true,
                    MessageId = evento.Id.ToString()
                },
                body: Encoding.UTF8.GetBytes(evento.Contenuto),
                cancellationToken: cancellationToken
            );

            evento.MarcaCompletato();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}