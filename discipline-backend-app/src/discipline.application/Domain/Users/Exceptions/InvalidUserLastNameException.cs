using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class InvalidUserLastNameException(string value)
    : DisciplineException($"User last name: {value} has invalid length");