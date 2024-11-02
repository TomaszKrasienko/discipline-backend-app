using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Events;

public interface IEventProcessor
{
    Task PublishAsync(params DomainEvent[] events);
}