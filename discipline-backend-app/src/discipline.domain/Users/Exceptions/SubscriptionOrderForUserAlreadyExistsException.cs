using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class SubscriptionOrderForUserAlreadyExistsException(Guid userId)
    : DisciplineException($"Subscription order for \"UserId\": {userId} already exists");