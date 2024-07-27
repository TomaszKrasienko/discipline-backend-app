using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class NullSubscriptionOrderFrequencyException()
    : DisciplineException("Subscription order frequency can not be null for paid subscription");