using discipline.centre.activityrules.domain.Specifications;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record UpdateActivityRuleDto(ActivityRuleDetailsSpecification Details, string Mode,
    List<int>? SelectedDays);