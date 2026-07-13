using OrderManagementCommon.Models;

namespace OrderManagementDeliveryService.Models;

public interface IOrderCancellabile { }
public interface IOrderCompletato { }
public interface IOrderAssigned
{
    public IdDeliveryGuy? IdDeliveryGuy { get; }
}

public abstract record OrderStatus;

public record OrderEvaded() : OrderStatus, IOrderCancellabile;
public record OrderAssegnato(IdDeliveryGuy IdDeliveryGuy) : OrderStatus, IOrderCancellabile, IOrderAssigned;
public record OrderCanceled(DateTime Momento, IdDeliveryGuy? IdDeliveryGuy) : OrderStatus, IOrderAssigned, IOrderCompletato;
public record OrderDelivered(DateTime Momento, IdDeliveryGuy IdDeliveryGuy) : OrderStatus, IOrderAssigned, IOrderCompletato;


public static class DeliveryStatusStateMachine
{
    extension(OrderEvaded orderEvaded)
    {
        public OrderAssegnato Assegna(IdDeliveryGuy id) => new(id);
    }

    extension(IOrderCancellabile orderCancellabile)
    {
        public OrderCanceled Cancel(DateTime now) => orderCancellabile switch
        {
            OrderAssegnato assegnato => new OrderCanceled(now, assegnato.IdDeliveryGuy),
            OrderEvaded _ => new OrderCanceled(now, null),
            _ => throw new Exception()
        };
    }

    extension(OrderAssegnato evaded)
    {
        public OrderDelivered Delivery(DateTime now) => new(now.ToUniversalTime(), evaded.IdDeliveryGuy);
    }
}