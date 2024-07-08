using discipline.application.Exceptions;

namespace discipline.application.Domain.ActivityRules.Exceptions;

public sealed class ModeUnavailableException(string value)
    : DisciplineException($"Mode with value: {value} is unavailable");