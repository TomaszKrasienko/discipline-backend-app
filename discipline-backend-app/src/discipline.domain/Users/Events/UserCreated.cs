using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.ValueObjects.Users;

namespace discipline.domain.Users.Events;

public sealed record UserCreated(UserId UserId, Email Email) : DomainEvent;