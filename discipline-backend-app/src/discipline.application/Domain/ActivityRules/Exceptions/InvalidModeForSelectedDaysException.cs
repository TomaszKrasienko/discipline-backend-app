using discipline.application.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.application.Domain.ActivityRules.Exceptions;

public sealed class InvalidModeForSelectedDaysException(string mode)
    : DisciplineException($"Selected mode: {mode} is invalid for selecting days");