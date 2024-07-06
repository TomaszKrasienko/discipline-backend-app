using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class EmptyEventTitleException()
    : DisciplineException("Event title can not be empty");