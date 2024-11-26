using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.application.Exceptions;

public sealed class SubscriptionNotFoundException(SubscriptionId subscriptionId)
    : DisciplineException($"Subscription with \"ID\": {subscriptionId.ToString()} does not exists");