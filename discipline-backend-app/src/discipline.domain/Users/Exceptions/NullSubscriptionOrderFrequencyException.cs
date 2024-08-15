using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class NullSubscriptionOrderFrequencyException()
    : DisciplineException("Subscription order frequency can not be null for paid subscription");