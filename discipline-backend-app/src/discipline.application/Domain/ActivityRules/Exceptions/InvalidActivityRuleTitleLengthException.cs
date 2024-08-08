using discipline.application.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.application.Domain.ActivityRules.Exceptions;

public sealed class InvalidActivityRuleTitleLengthException(string value)
    : DisciplineException($"Activity rule title: {value} has invalid length");