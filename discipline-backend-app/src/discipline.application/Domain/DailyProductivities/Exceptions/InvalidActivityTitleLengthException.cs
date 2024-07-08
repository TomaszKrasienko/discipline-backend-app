using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class InvalidActivityTitleLengthException(string value)
    : DisciplineException($"Activity title: {value} has invalid length");
