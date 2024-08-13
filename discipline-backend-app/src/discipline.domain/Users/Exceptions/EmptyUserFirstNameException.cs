using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptyUserFirstNameException()
    : DisciplineException("User first name can not be empty");