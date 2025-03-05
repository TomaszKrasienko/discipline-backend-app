using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.Events;

public record ActivityRuleCreated(ActivityRuleId ActivityRuleId,
    UserId UserId) : DomainEvent;