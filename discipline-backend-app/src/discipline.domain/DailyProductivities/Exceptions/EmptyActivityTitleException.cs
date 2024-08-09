using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Exceptions;

public sealed class EmptyActivityTitleException() 
    : DisciplineException("Activity title can not be null or empty");