using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users.ValueObjects.Users;

namespace discipline.centre.users.domain.Users.Events;

public sealed record UserCreated(UserId UserId, Email Email) : DomainEvent;