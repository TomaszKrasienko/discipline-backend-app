using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptyUserEmailException()
    : DisciplineException("User email can not be empty");