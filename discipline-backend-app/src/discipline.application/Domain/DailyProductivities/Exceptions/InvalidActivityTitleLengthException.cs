using discipline.application.Exceptions;

namespace discipline.application.Domain.DailyProductivities.Exceptions;

public sealed class InvalidActivityTitleLengthException(string value)
    : DisciplineException($"Activity title: {value} has invalid length");
