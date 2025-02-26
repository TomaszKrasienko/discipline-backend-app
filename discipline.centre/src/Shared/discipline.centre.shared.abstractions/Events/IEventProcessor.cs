namespace discipline.centre.shared.abstractions.Events;

public interface IEventProcessor
{
    Task PublishAsync(params IEvent[] events);
}