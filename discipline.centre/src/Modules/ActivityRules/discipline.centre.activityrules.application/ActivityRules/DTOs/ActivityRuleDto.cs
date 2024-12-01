namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record ActivityRuleDto
{
    public Ulid Id { get; init; }
    public required string Title { get; init; }
    public string? Note { get; init; }
    public required string Mode { get; init; }
    public List<int>? SelectedDays { get; init; }
}