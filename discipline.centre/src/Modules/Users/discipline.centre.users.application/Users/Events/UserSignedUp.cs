using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.application.Users.Events;

public sealed record UserSignedUp(UserId UserId, string Email) : IEvent;