using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public abstract class AuthorizeException(
    string message) : DisciplineException(message);