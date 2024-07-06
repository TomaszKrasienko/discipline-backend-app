namespace discipline.application.Exceptions;

public sealed class ActivityRuleNotFoundException(Guid id)
    : DisciplineException($"Activity rule with \"ID\": {id} does not exist");