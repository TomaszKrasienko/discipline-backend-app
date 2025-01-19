using discipline.centre.dailytrackers.domain;

namespace discipline.centre.dailytrackers.application.DailyTrackers.DTOs;

/// <summary>
/// Data transfer object (DTO) representing the daily tracker information.
/// </summary>
/// <param name="Day">The date of the daily tracker entry</param>
/// <param name="Activities">A collection of activities associated with the specified day</param>
public sealed record DailyTrackerDto(DateOnly Day, IReadOnlyCollection<ActivityDto> Activities);
