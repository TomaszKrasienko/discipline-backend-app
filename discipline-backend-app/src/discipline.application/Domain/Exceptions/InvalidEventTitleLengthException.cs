using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class InvalidEventTitleLengthException(string value)
    : DisciplineException($"Event title: {value} has invalid length");