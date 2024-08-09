using discipline.domain.SharedKernel;

namespace discipline.domain.ActivityRules.Exceptions;

internal sealed class SelectedDayOutOfRangeException(int day)
    : DisciplineException($"Selected day: {day} is out of week range");