using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class NullSubscriptionException()
    : DisciplineException("Subscription can not be null");