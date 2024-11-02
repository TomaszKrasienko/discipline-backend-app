using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.users.domain.Users.Events;

public sealed record FreeSubscriptionPicked(
    Ulid UserId, Ulid SubscriptionId) : DomainEvent;