namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record ActiveModesDto
{
    public required List<string> Values { get; init; }
}