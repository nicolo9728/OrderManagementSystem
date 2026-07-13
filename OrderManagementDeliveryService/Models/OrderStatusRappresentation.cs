using OrderManagementCommon.Models;

namespace OrderManagementDeliveryService.Models;

public record OrderStatusRappresentation(
    DateTime? MomentoCancellazione,
    DateTime? MomentoConsegna,
    IdDeliveryGuy? IdDeliveryGuyAssegnato,
    string Tipo);

public static class OrderStatusRappresentationConverter
{
    extension(OrderStatusRappresentation rappresentation)
    {
        public OrderStatus ToDomain()
            => rappresentation.Tipo switch
            {
                "OrderEvaded" => new OrderEvaded(),
                "OrderAssegnato" => new OrderAssegnato(rappresentation.IdDeliveryGuyAssegnato!),
                "OrderCanceled" => new OrderCanceled(
                    rappresentation.MomentoCancellazione!.Value,
                    rappresentation.IdDeliveryGuyAssegnato),
                "OrderDelivered" => new OrderDelivered(rappresentation.MomentoConsegna!.Value, rappresentation.IdDeliveryGuyAssegnato!),
                _ => throw new Exception()
            };

        public static OrderStatusRappresentation FromDomain(OrderStatus status)
            => status switch
            {
                OrderEvaded _ => new OrderStatusRappresentation(null, null, null, "OrderEvaded"),
                OrderAssegnato ass => new OrderStatusRappresentation(null, null, ass.IdDeliveryGuy, "OrderAssegnato"),
                OrderCanceled canc => new OrderStatusRappresentation(canc.Momento, null, canc.IdDeliveryGuy, "OrderCanceled"),
                OrderDelivered del => new OrderStatusRappresentation(null, del.Momento, del.IdDeliveryGuy, "OrderDelivered"),
                _ => throw new Exception()
            };
    }
}