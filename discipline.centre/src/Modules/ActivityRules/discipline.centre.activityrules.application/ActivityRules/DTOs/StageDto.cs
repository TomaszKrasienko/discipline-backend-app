using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record StageDto
{
    public required StageId StageId { get; init; }
    public required string Title { get; init; }
    public int Index { get; init; }
}