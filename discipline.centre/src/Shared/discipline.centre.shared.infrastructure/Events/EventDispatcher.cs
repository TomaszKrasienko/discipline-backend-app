using discipline.centre.shared.abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
        using var scope = serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventHandler<TEvent>>>();

        var tasks = handlers.Select(x => x.HandleAsync(@event));
        await Task.WhenAll(tasks);
    }
}