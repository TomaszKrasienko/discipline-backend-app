using discipline.domain.SharedKernel;

namespace discipline.domain.ActivityRules.Exceptions;

public sealed class ModeUnavailableException(string value)
    : DisciplineException($"Mode with value: {value} is unavailable");