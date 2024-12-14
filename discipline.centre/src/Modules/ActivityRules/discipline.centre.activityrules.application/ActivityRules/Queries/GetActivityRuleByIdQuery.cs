using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Queries;

public sealed record GetActivityRuleByIdQuery(ActivityRuleId ActivityRuleId, UserId UserId) : IQuery<ActivityRuleDto?>;