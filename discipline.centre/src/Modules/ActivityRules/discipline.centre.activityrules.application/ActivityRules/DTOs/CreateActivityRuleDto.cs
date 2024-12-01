namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record CreateActivityRuleDto(string Title, string? Note, string Mode,
    List<int>? SelectedDays);