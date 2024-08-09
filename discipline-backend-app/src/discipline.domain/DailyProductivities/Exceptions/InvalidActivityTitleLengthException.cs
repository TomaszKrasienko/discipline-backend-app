using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Exceptions;

public sealed class InvalidActivityTitleLengthException(string value)
    : DisciplineException($"Activity title: {value} has invalid length");
