using System.Diagnostics;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.DTOs;

/// <summary>
/// Data Transfer Object representing a <see cref="Activity"/>
/// </summary>
public sealed record ActivityDto
{
    /// <summary>
    /// Unique identifier of the <see cref="Activity"/>
    /// </summary>
    public required ActivityId ActivityId { get; init; }
    
    /// <summary>
    /// Details of the <see cref="Details"/>. For more, see <see cref="ActivityDetailsSpecification"/>.
    /// </summary>
    public required ActivityDetailsSpecification Details { get; init; }
    
    /// <summary>
    /// Indicates whether the <see cref="Activity"/> is checked.
    /// </summary>
    public bool IsChecked { get; init; }
    
    /// <summary>
    /// Represents optional 'ActivityRule' as a parent of <see cref="Activity"/>.
    /// </summary>
    public ActivityRuleId? ParentActivityRuleId { get; init; }
    
    /// <summary>
    /// Collection of <see cref="StageDto"/>.
    /// </summary>
    public IReadOnlyCollection<StageDto>? Stages { get; init; }
}