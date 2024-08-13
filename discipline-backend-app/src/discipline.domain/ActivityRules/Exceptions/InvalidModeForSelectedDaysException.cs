using discipline.domain.SharedKernel;

namespace discipline.domain.ActivityRules.Exceptions;

public sealed class InvalidModeForSelectedDaysException(string mode)
    : DisciplineException($"Selected mode: {mode} is invalid for selecting days");