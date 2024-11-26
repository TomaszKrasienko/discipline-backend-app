using discipline.domain.SharedKernel;
using discipline.domain.Users.Enums;

namespace discipline.domain.Users.Exceptions;

public sealed class InvalidFrequencyException(SubscriptionOrderFrequency subscriptionOrderFrequency)
    : DisciplineException($"Subscription frequency: {subscriptionOrderFrequency} is invalid");