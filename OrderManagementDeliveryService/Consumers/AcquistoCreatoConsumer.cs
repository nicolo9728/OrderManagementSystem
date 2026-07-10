using OrderManagementCommon.Events;
using OrderManagementDeliveryService.Database;
using OrderManagementDeliveryService.Models;

namespace OrderManagementDeliveryService.Consumers;

public class AcquistoCreatoConsumer(DeliveryServiceDbContext context) : IEventConsumer<AcquistoCreatoEvent>
{
    public async Task Consume(AcquistoCreatoEvent arg, CancellationToken cancellationToken)
    {
        Order order = new(arg.IdProdotto, arg.IdCustomer, arg.QuantitaAcquista, arg.MomentoAcquisto);

        await context.Ordini.AddAsync(order, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);

    }
}