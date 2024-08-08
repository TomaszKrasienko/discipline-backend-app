using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptyUserEmailException()
    : DisciplineException("User email can not be empty");