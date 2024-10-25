using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Events;

public sealed record PaidSubscriptionPicked(
    Ulid UserId, Ulid SubscriptionId, DateOnly Next) : DomainEvent;