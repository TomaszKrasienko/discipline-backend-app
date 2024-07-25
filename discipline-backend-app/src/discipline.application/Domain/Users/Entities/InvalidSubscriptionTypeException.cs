using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Entities;

public sealed class InvalidSubscriptionTypeException()
    : DisciplineException("Subscription type is invalid for subscriptionOrder");