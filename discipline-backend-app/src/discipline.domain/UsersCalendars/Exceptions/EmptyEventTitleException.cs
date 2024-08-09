using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class EmptyEventTitleException()
    : DisciplineException("Event title can not be empty");