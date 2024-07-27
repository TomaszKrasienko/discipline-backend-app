using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class NullUserException()
    :   DisciplineException("User can not be null");