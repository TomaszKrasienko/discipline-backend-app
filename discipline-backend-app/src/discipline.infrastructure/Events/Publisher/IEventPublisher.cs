using discipline.application.Behaviours;
using discipline.application.Behaviours.Events;

namespace discipline.infrastructure.Events.Publisher;

internal interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class, IEvent;
}