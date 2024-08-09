using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class InvalidEventTitleLengthException(string value)
    : DisciplineException($"Event title: {value} has invalid length");