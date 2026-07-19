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
        //il while true è dovuto al fatto che all'inizio la coda di rabbitmq potrebbe non essere ancora attiva in questo modo evito che l'app crashi
        while (true)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMQ:Hostname"]!,
                    UserName = configuration["RabbitMQ:Username"]!,
                    Password = configuration["RabbitMQ:Password"]!
                };

                using var connection = await factory.CreateConnectionAsync(stoppingToken);
                var channel = await connection.CreateChannelAsync(
                    new CreateChannelOptions(
                        publisherConfirmationsEnabled: true,
                        publisherConfirmationTrackingEnabled: true), stoppingToken);

                //Dichiara l'exchange nel caso non sia gia creato
                await channel.ExchangeDeclareAsync(
                    exchange: configuration["RabbitMQ:ExchangeName"]!,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    cancellationToken: stoppingToken
                );


                while (!stoppingToken.IsCancellationRequested)
                {
                    //Per ogni messaggio nella tabella dei messaggi del database viene pubblicato il messaggio nella coda
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
            catch (Exception)
            {
                await Task.Delay(1000, stoppingToken);
            }
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

        if (eventiPendenti.Count > 0)
        {
            //per ogni evento nella tabella degli eventi del database
            foreach (var evento in eventiPendenti)
            {
                //pubblico l'evento nel database
                await channel.BasicPublishAsync(
                    exchange: configuration["RabbitMQ:ExchangeName"]!,
                    routingKey: evento.Tipo, //la chiave è in base al Tipo dell'evento
                    mandatory: true, //garantisce che il messaggio non vada perso nel caso rabbitmq non riesca ad instradarlo subito
                    basicProperties: new BasicProperties()
                    {
                        Persistent = true,
                        MessageId = evento.Id.ToString()
                    },
                    body: Encoding.UTF8.GetBytes(evento.Contenuto),//il body del messaggio e la serializzazione in json dell'evento
                    cancellationToken: cancellationToken
                );

                evento.MarcaCompletato();
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}