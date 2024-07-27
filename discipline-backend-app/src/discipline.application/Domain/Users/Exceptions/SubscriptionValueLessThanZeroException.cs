using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class SubscriptionValueLessThanZeroException(string name)
    : DisciplineException($"Subscription: {name} can not be less then zero");