using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.users.domain.Users.Events;

public sealed record FreeSubscriptionPicked(
    Ulid UserId, Ulid SubscriptionId) : DomainEvent;