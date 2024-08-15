using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class InvalidUserFirstNameException(string value)
    : DisciplineException($"User first name: {value} has invalid length");