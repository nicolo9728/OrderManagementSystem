namespace OrderManagementCommon.Models;

public abstract record DomainEvent;

public abstract class AggregateRoot
{
    private readonly List<DomainEvent> domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => domainEvents;

    public void AddDomainEvent(DomainEvent e)
        => domainEvents.Add(e);
    
    public void ClearDomainEvents()
        => domainEvents.Clear();
}