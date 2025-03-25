using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

public sealed record ActivityRuleRegistered(string ActivityRuleId,
    string UserId) : IEvent;