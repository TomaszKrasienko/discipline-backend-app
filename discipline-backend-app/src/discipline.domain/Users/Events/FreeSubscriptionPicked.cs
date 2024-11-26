using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Events;

public sealed record FreeSubscriptionPicked(
    Ulid UserId, Ulid SubscriptionId) : DomainEvent;