using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.infrastructure.Events.Brokers.Abstractions;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventProcessor(
    IRedisPubSubClient redisPubSubClient,
    ISerializer serializer) : IEventProcessor
{
    public async Task PublishAsync(params IEvent[] domainEvents)
    {
        foreach (var @event in domainEvents)
        {
            var json = serializer.ToJson(@event);
            await redisPubSubClient.SendAsync(json, @event.GetType().Name);
        }
    }
}