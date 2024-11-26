using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventProcessor(
    IEventMapper mapper) : IEventProcessor
{
    public Task PublishAsync(params DomainEvent[] domainEvents)
    {
        List<IEvent> events = new List<IEvent>();
        foreach (var @event in domainEvents)
        {
            events.Add(mapper.MapAsEvent(@event));
        }
            
        return Task.CompletedTask;
    }
}