using discipline.domain.SharedKernel;

namespace discipline.application.Behaviours.Events;

/// <summary>
/// Marker
/// </summary>
public interface IEvent;

public interface IEventProcessor
{
    Task PublishAsync(params DomainEvent[] events);
}