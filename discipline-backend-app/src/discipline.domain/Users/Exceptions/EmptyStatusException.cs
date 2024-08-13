using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptyStatusException()
    : DisciplineException("Status can not be empty");