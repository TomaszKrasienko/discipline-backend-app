using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptyStatusException()
    : DisciplineException("Status can not be empty");