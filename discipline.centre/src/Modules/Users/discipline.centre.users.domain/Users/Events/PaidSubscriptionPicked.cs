using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.users.domain.Users.Events;

public sealed record PaidSubscriptionPicked(
    Ulid UserId, Ulid SubscriptionId, DateOnly Next) : DomainEvent;