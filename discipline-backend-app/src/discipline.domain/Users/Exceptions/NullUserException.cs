using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class NullUserException()
    :   DisciplineException("User can not be null");