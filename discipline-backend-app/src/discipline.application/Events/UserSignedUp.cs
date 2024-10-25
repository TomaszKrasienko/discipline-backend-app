using discipline.application.Behaviours;

namespace discipline.application.Events;

public sealed record UserSignedUp(Ulid UserId, string Email) : IEvent;