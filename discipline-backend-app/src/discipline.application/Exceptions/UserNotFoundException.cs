namespace discipline.application.Exceptions;

public sealed class UserNotFoundException(Guid userId)
    : DisciplineException($"User with \"ID\": {userId} does not exists");