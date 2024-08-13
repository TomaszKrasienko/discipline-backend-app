using discipline.domain.SharedKernel;

namespace discipline.domain.ActivityRules.Exceptions;

public sealed class EmptyActivityRuleModeException() 
    : DisciplineException("Activity rule mode can not be null or empty");