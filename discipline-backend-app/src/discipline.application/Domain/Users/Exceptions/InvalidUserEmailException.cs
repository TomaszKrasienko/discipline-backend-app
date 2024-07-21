using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class InvalidUserEmailException(string value)
    :   DisciplineException($"User email: {value} is invalid");