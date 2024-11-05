using discipline.centre.shared.abstractions.Events;

namespace discipline.centre.users.application.Users.Events;

public sealed record UserSignedUp(Ulid UserId, string Email) : IEvent;