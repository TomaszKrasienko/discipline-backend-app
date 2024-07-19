using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class InvalidUserFirstNameException(string value)
    : DisciplineException($"User first name: {value} has invalid length");