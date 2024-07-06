using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class ModeUnavailableException(string value)
    : DisciplineException($"Mode with value: {value} is unavailable");