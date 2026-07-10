namespace OrderManagementCommon.Events;

public interface IEventConsumer<T> where T: DomainEvent
{
    public Task Consume(T arg, CancellationToken cancellationToken);
}