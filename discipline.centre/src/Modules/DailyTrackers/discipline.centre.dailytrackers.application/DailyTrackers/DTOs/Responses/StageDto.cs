using discipline.centre.dailytrackers.domain;

namespace discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;

/// <summary>
/// Data Transfer Object representing a <see cref="Stage"/>.
/// </summary>
public sealed record StageDto
{
    /// <summary>
    /// Unique identifier of the <see cref="Stage"/>.
    /// </summary>
    public required string StageId { get; init; }
    
    /// <summary>
    /// Title of the <see cref="Stage"/>.
    /// </summary>
    public required string Title { get; init; }
    
    /// <summary>
    /// Display order index of the <see cref="Stage"/>.
    /// </summary>
    public int Index { get; init; }
    
    /// <summary>
    /// Indicates whether the <see cref="Stage"/> is checked.
    /// </summary>
    public bool IsChecked { get; init; }
}