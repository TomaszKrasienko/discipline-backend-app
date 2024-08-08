using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class InvalidUserEmailException(string value)
    :   DisciplineException($"User email: {value} is invalid");