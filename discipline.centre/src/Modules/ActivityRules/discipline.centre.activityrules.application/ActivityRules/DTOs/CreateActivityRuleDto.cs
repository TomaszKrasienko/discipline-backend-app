namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record CreateActivityRuleDto(string Title, string Mode,
    List<int>? SelectedDays);