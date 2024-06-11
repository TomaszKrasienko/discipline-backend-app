using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class EmptyActivityRuleTitleException() 
    : DisciplineException("Activity rule title can not be null or empty");