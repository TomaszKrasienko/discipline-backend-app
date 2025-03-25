using discipline.centre.shared.abstractions.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventDispatcher(IServiceProvider serviceProvider,
    ILogger<EventDispatcher> logger) : IEventDispatcher
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
        using var scope = serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventHandler<TEvent>>>();

        logger.LogInformation("Publishing {0}", typeof(TEvent).Name);
        
        var tasks = handlers.Select(x => x.HandleAsync(@event, CancellationToken.None));
        await Task.WhenAll(tasks);
    }
}