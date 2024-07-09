using discipline.application.Exceptions;

namespace discipline.application.Domain.ActivityRules.Exceptions;

public sealed class EmptyActivityRuleTitleException() 
    : DisciplineException("Activity rule title can not be null or empty");