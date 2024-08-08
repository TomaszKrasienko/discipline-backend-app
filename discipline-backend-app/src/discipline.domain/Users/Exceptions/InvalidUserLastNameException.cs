using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class InvalidUserLastNameException(string value)
    : DisciplineException($"User last name: {value} has invalid length");