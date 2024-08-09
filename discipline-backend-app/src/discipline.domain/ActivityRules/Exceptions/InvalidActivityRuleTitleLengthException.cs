using discipline.domain.SharedKernel;

namespace discipline.domain.ActivityRules.Exceptions;

public sealed class InvalidActivityRuleTitleLengthException(string value)
    : DisciplineException($"Activity rule title: {value} has invalid length");