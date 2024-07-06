using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

internal sealed class SelectedDayOutOfRangeException(int day)
    : DisciplineException($"Selected day: {day} is out of week range");