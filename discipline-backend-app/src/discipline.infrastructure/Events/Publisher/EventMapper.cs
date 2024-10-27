using discipline.application.Behaviours;
using discipline.application.Behaviours.Events;
using discipline.application.Events;
using discipline.domain.SharedKernel;
using discipline.domain.Users.Events;

namespace discipline.infrastructure.Events.Publisher;

//TODO: Tests
internal static class EventMapper
{
    internal static IEvent MapAsEvent(this DomainEvent @event)
        => @event switch
        {
            UserCreated domainEvent => new UserSignedUp(domainEvent.UserId.Value, domainEvent.Email),
            _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null)
        };
}