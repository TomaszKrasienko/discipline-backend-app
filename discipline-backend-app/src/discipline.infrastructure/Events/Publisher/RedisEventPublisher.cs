using discipline.application.Behaviours;
using discipline.application.Behaviours.Events;
using discipline.infrastructure.Serialization;
using StackExchange.Redis;

namespace discipline.infrastructure.Events.Publisher;

internal sealed class RedisEventPublisher(
    ISubscriber subscriber,
    IEventsChannelConventionProvider routesConventionProvider) : IEventPublisher
{
    public async Task PublishAsync<T>(T @event) where T : class, IEvent
    {
        var channel = routesConventionProvider.Get(@event.GetType());
        await subscriber.PublishAsync( channel, @event.AsJson());
    }
}