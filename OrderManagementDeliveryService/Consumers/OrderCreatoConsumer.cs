using Microsoft.EntityFrameworkCore;
using OrderManagementCommon;
using OrderManagementCommon.Events;
using OrderManagementCommon.Models;
using OrderManagementDeliveryService.Database;
using OrderManagementDeliveryService.Models;

namespace OrderManagementDeliveryService.Consumers;

public class OrderCreatoConsumer(DeliveryServiceDbContext context) : IEventConsumer<OrderCreatoEvent>
{

    private async Task<DeliveryGuy> GetDeliveryGuyWithLessOrderAssigned()
        => await context.DeliveryGuys
            .OrderBy((d)=>d.NumeroConsegneAttive)
            .Take(1)
            .FirstAsync();

    public async Task Consume(OrderCreatoEvent arg, CancellationToken cancellationToken)
    {
        Order? order = await context.Ordini.Where((o)=>o.Id == arg.Id).FirstOrDefaultAsync(cancellationToken);

        if(order == null)
            return;
        
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var deliveryGuyDaAssegnare = await GetDeliveryGuyWithLessOrderAssigned();

        order.TryAssegna(deliveryGuyDaAssegnare.Id);

        if(order.Status is OrderAssegnato)
            deliveryGuyDaAssegnare.SegnalaNuovaConsegna();

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}