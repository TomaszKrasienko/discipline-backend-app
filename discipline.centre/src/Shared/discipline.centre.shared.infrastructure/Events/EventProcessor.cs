using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Abstractions;
using discipline.centre.shared.infrastructure.Events.Brokers.Redis.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventProcessor(
    IServiceProvider serviceProvider,
    ISerializer serializer) : IEventProcessor
{
    public async Task PublishAsync(params IEvent[] domainEvents)
    {
        using var scope = serviceProvider.CreateScope();
        var redisPubSubClient = scope.ServiceProvider.GetRequiredService<IRedisPubSubClient>();
        var internalDispatcher = scope.ServiceProvider.GetRequiredService<IAsyncMessageDispatcher>();
        
        foreach (var @event in domainEvents)
        {
            //TODO: Cancellation Token
            var json = serializer.ToJson(@event);
            await redisPubSubClient.SendAsync(json, @event.GetType().Name);
            await internalDispatcher.PublishAsync(@event);
        }
    }
}