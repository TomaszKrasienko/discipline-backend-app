namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record ActiveModesDto
{
    public required List<string> Modes { get; init; }
    public int Day { get; init; }
}