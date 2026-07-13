using Microsoft.EntityFrameworkCore;
using OrderManagementCommon.Events;
using OrderManagementDeliveryService.Database;
using OrderManagementDeliveryService.Models;

namespace OrderManagementDeliveryService.Consumers;

public class OrderCompletatoConsumer(DeliveryServiceDbContext context) : IEventConsumer<OrderCompletatoEvent>
{
    public async Task Consume(OrderCompletatoEvent arg, CancellationToken cancellationToken)
    {
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        Order order = await context.Ordini.Where((o)=>o.Id == arg.Id).FirstAsync(cancellationToken);
        
        if(order.IsDeliveryGuyNotified)
            return;
        
        DeliveryGuy deliveryGuy = await context.DeliveryGuys.Where((d)=>d.Id == order.IdDeliveryGuyAssigned).FirstAsync(cancellationToken);

        deliveryGuy.SegnalaConsegnaConclusa();
        order.MarkDeliveryGuyNotified();
        
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}