using discipline.application.Exceptions;

namespace discipline.application.Domain.ActivityRules.Exceptions;

public sealed class EmptyActivityRuleModeException() 
    : DisciplineException("Activity rule mode can not be null or empty");