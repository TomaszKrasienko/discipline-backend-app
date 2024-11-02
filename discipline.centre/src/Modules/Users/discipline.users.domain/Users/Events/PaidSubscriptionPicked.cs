using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.users.domain.Users.Events;

public sealed record PaidSubscriptionPicked(
    Ulid UserId, Ulid SubscriptionId, DateOnly Next) : DomainEvent;