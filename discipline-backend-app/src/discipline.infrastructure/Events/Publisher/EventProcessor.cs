using discipline.application.Behaviours;
using discipline.domain.SharedKernel;

namespace discipline.infrastructure.Events.Publisher;


//TODO: Tests
internal sealed class EventProcessor(
    IEventPublisher eventPublisher) : IEventProcessor
{
    public async Task PublishAsync(params DomainEvent[] domainEvents)
    {
        var events = domainEvents.Select(x => x.MapAsEvent());
        foreach (var @event in events)
        {
            await eventPublisher.PublishAsync(@event);
        }
    }
}