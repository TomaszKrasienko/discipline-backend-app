using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;

public sealed record ActivityRuleDto
{
    public required ActivityRuleId ActivityRuleIdId { get; init; }
    public required string Title { get; init; }
    public string? Note { get; init; }
    public required string Mode { get; init; }
    public IReadOnlyList<int>? SelectedDays { get; init; }
    public IReadOnlyList<StageDto>? Stages { get; init; }
}