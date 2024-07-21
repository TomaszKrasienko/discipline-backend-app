using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptyUserFirstNameException()
    : DisciplineException("User first name can not be empty");