using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class EmailAlreadyExistsException(string email)
    : DisciplineException($"Email: {email} already registered");