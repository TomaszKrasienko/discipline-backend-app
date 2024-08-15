using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptyUserLastNameException()
    :  DisciplineException("User last name can not be empty");