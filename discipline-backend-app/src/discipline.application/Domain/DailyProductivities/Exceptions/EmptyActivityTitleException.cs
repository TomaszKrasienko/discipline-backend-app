using discipline.application.Exceptions;

namespace discipline.application.Domain.DailyProductivities.Exceptions;

public sealed class EmptyActivityTitleException() 
    : DisciplineException("Activity title can not be null or empty");