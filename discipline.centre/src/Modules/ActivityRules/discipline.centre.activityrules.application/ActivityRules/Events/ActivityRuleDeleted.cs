using discipline.centre.shared.abstractions.Events;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

public sealed record ActivityRuleDeleted(Ulid userId, Ulid activityRuleId) : IEvent;