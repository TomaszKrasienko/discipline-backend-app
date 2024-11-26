using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.application.Users.Events;
using discipline.centre.users.domain.Users.Events;

namespace discipline.centre.users.infrastructure.Events;

internal sealed class UsersEventMapper : IEventMapper
{
    public IEvent MapAsEvent(DomainEvent domainEvent) => domainEvent switch
    {
        UserCreated @event => new UserSignedUp(@event.UserId.Value, @event.Email),
        _ => throw new ArgumentOutOfRangeException(nameof(domainEvent), domainEvent, null)
    };
}