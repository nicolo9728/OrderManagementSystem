using OrderManagementCommon.Models;

namespace OrderManagementDeliveryService.Models;

public record IdOrder(Guid Valore);

public class Order
{
    public IdOrder Id { get; }
    public IdProdotto IdProdotto { get; }
    public IdCustomer IdUtente { get; }
    public Quantita Quantita { get; }
    public OrderStatus Status { get; private set; }

    public Order(IdProdotto idProdotto, IdCustomer idUtente, Quantita quantita)
    {
        IdProdotto = idProdotto;
        IdUtente = idUtente;
        Quantita = quantita;
        Status = new OrderEvaded();
        Id = new IdOrder(Guid.NewGuid());
    }

    private Order()
    {
        Quantita = null!;
        Status = null!;
        IdProdotto = null!;
        IdUtente = null!;
        Id = null!;
    }

    public bool CanDelivery => Status is OrderEvaded;
    public bool CanCancel => Status is OrderEvaded;
    public bool CanAssegnare => Status is OrderEvaded;

    public void TryCancel(DateTime now, string ragione)
        => Status = Status is IOrderCancellabile evaded ? evaded.Cancel(now, ragione) : Status;

    public void TryDelivery(DateTime now)
        => Status = Status is OrderAssegnato assegnato ? assegnato.Delivery(now) : Status;

    public void TryAssegna(IdDeliveryGuy id)
        => Status = Status is OrderEvaded evaded ? evaded.Assegna(id) : Status;

    private OrderStatusRappresentation OrderStatusRappresentation
    {
        get => OrderStatusRappresentation.FromDomain(Status);
        set => Status = value.ToDomain();
    }
}
