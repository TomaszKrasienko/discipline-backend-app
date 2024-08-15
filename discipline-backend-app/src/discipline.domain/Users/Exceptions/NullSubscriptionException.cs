using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class NullSubscriptionException()
    : DisciplineException("Subscription can not be null");