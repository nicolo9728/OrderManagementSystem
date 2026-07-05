using OrderManagementCommon.Events;

namespace OrderManagementCommon.Models;

public abstract class AggregateRoot
{
    private readonly List<DomainEvent> domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => domainEvents;

    public void AddDomainEvent(DomainEvent e)
        => domainEvents.Add(e);
    
    public void ClearDomainEvents()
        => domainEvents.Clear();
}