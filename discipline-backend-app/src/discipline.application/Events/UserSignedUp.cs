using discipline.application.Behaviours;
using discipline.application.Behaviours.Events;

namespace discipline.application.Events;

public sealed record UserSignedUp(Ulid UserId, string Email) : IEvent;