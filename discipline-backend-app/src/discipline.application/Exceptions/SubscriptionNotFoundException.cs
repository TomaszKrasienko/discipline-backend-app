using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class SubscriptionNotFoundException(Guid subscriptionId)
    : DisciplineException($"Subscription with \"ID\": {subscriptionId} does not exists");