using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class SubscriptionOrderForUserAlreadyExistsException(Guid userId)
    : DisciplineException($"Subscription order for \"UserId\": {userId} already exists");