using discipline.centre.shared.abstractions.Events;

namespace discipline.centre.shared.infrastructure.Events.Brokers.Internal.Abstractions;

public interface IAsyncMessageDispatcher
{
    Task PublishAsync<T>(T message) where T : class, IEvent;
}