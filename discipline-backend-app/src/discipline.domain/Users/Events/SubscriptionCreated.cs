using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.Users.Events;

public sealed record SubscriptionCreated(SubscriptionId Id, string Title) : DomainEvent;