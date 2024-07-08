using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class EmptyActivityRuleModeException() 
    : DisciplineException("Activity rule mode can not be null or empty");