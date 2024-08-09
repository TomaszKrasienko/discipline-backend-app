using discipline.domain.SharedKernel;

namespace discipline.domain.ActivityRules.Exceptions;

public sealed class EmptyActivityRuleTitleException() 
    : DisciplineException("Activity rule title can not be null or empty");