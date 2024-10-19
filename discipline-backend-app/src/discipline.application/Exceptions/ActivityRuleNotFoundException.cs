using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.application.Exceptions;

public sealed class ActivityRuleNotFoundException(ActivityRuleId id)
    : DisciplineException($"Activity rule with \"ID\": {id} does not exist");