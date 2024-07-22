using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class NullSubscriptionOrderFrequencyException(Guid subscriptionId)
    : DisciplineException($"Subscription order frequency can not be null for subscription with \"ID\": {subscriptionId}");