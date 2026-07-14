using OrderManagementCommon.Events;
using OrderManagementDeliveryService.Database;
using OrderManagementDeliveryService.Models;

namespace OrderManagementDeliveryService.Consumers;

public class DeliveryGuyCreatoConsumer(DeliveryServiceDbContext context) : IEventConsumer<DeliveryGuyCreatoEvent>
{
    public async Task Consume(DeliveryGuyCreatoEvent arg, CancellationToken cancellationToken)
    {
        DeliveryGuy deliveryGuy = new(arg.Id);

        await context.AddAsync(deliveryGuy, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}