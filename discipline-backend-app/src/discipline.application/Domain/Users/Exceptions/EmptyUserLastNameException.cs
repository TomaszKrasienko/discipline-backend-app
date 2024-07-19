using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptyUserLastNameException()
    :  DisciplineException("User last name can not be empty");