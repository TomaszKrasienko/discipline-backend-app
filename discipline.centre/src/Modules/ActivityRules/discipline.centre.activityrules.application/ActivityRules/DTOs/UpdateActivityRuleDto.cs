namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record UpdateActivityRuleDto(string Title, string? Note, string Mode,
    List<int>? SelectedDays);