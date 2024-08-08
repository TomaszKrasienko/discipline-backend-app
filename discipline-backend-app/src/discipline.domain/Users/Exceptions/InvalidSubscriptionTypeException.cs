using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class InvalidSubscriptionTypeException()
    : DisciplineException("Subscription type is invalid for subscriptionOrder");