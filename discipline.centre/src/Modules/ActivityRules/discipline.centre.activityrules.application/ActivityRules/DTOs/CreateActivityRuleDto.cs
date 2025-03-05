using discipline.centre.activityrules.domain.Specifications;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record CreateActivityRuleDto(ActivityRuleDetailsSpecification Details, string Mode,
    List<int>? SelectedDays, List<StageSpecification>? Stages);