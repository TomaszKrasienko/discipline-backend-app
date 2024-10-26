using discipline.application.Behaviours;

namespace discipline.infrastructure.Events.Publisher;

internal interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class, IEvent;
}