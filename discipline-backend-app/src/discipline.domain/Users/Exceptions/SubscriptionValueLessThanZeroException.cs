using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class SubscriptionValueLessThanZeroException(string name)
    : DisciplineException($"Subscription: {name} can not be less then zero");