using discipline.centre.shared.abstractions.Events;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Logging.Decorators;

internal sealed class LoggingEventHandlerDecorator<TEvent>(
    IEventHandler<TEvent> handler,
    ILogger<IEventHandler<TEvent>> logger) : IEventHandler<TEvent> where TEvent : class, IEvent
{
    public async Task HandleAsync(TEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling event: {0}", @event.GetType().Name);
        
        try
        {
            await handler.HandleAsync(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}