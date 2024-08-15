using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class InvalidPasswordException()
    : DisciplineException("The password is invalid");