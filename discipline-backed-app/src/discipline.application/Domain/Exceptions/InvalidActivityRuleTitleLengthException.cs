using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class InvalidActivityRuleTitleLengthException(string value)
    : DisciplineException($"Activity rule title: {value} has invalid length");