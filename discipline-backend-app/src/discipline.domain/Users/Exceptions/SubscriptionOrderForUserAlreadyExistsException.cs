using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.Users.Exceptions;

public sealed class SubscriptionOrderForUserAlreadyExistsException(UserId userId)
    : DisciplineException($"Subscription order for \"UserId\": {userId.ToString()} already exists");