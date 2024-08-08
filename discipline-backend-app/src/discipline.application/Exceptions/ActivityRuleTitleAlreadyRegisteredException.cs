using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class ActivityRuleTitleAlreadyRegisteredException(string title) 
    : DisciplineException($"Activity rule title: {title} is already registered");