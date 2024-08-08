using discipline.application.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.application.Domain.DailyProductivities.Exceptions;

public sealed class EmptyActivityTitleException() 
    : DisciplineException("Activity title can not be null or empty");