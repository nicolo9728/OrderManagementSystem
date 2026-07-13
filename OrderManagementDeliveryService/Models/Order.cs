using OrderManagementCommon.Events;
using OrderManagementCommon.Models;

namespace OrderManagementDeliveryService.Models;

public record IdOrder(Guid Valore);

public record OrderCreatoEvent(IdOrder Id) : DomainEvent;
public record OrderCompletatoEvent(IdOrder Id) : DomainEvent;

public class Order : AggregateRoot
{
    public IdOrder Id { get; }
    public IdProdotto IdProdotto { get; }
    public IdCustomer IdUtente { get; }
    public DateTime MomentoAcquisto { get; }
    public Quantita Quantita { get; }
    public OrderStatus Status { get; private set; }
    public Indirizzo Indirizzo { get; }
    public bool IsDeliveryGuyNotified { get; private set; }

    public IdDeliveryGuy? IdDeliveryGuyAssigned =>
        Status is IOrderAssigned assigned ? assigned.IdDeliveryGuy : null;

    public Order(IdProdotto idProdotto, IdCustomer idUtente, Quantita quantita, DateTime momento, Indirizzo indirizzo)
    {
        IdProdotto = idProdotto;
        IdUtente = idUtente;
        Quantita = quantita;
        MomentoAcquisto = momento;
        Status = new OrderEvaded();
        Id = new IdOrder(Guid.NewGuid());
        Indirizzo = indirizzo;

        AddDomainEvent(new OrderCreatoEvent(Id));
    }

    private Order()
    {
        Quantita = null!;
        Status = null!;
        IdProdotto = null!;
        IdUtente = null!;
        Id = null!;
        Indirizzo = null!;
    }

    public bool CanDelivery => Status is OrderEvaded;
    public bool CanCancel => Status is OrderEvaded;
    public bool CanAssegnare => Status is OrderEvaded;

    public void TryCancel(DateTime now)
    {
        if (Status is IOrderCancellabile orderCancellabile)
        {
            Status = orderCancellabile.Cancel(now);
            AddDomainEvent(new OrderCompletatoEvent(Id));
        }
    }

    public void TryDelivery(DateTime now)
    {
        if (Status is OrderAssegnato assegnato)
        {
            Status = assegnato.Delivery(now);
            AddDomainEvent(new OrderCompletatoEvent(Id));
        }
    }

    public void TryAssegna(IdDeliveryGuy id)
        => Status = Status is OrderEvaded evaded ? evaded.Assegna(id) : Status;

    public bool IsDeliveryGuyAssignedToThisOrder(IdDeliveryGuy id)
        => Status is IOrderAssigned orderAssigned && orderAssigned.IdDeliveryGuy == id;

    private OrderStatusRappresentation OrderStatusRappresentation
    {
        get => OrderStatusRappresentation.FromDomain(Status);
        set => Status = value.ToDomain();
    }

    public void MarkDeliveryGuyNotified()
    {
        if (Status is IOrderCompletato)
            IsDeliveryGuyNotified = true;
    }
}
