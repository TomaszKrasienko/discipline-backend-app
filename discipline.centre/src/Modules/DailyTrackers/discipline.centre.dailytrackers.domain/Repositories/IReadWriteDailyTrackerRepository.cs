using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.Repositories;

public interface IReadWriteDailyTrackerRepository
{
    Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken);
    Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken);
    Task UpdateRangeAsync(IEnumerable<DailyTracker> dailyTrackers, CancellationToken cancellationToken);
    Task DeleteAsync(DailyTracker dailyTracker, CancellationToken cancellationToken);
    Task<DailyTracker?> GetDailyTrackerByDayAsync(UserId userId, DateOnly day, CancellationToken cancellationToken);
    Task<DailyTracker?> GetDailyTrackerByIdAsync(UserId userId, DailyTrackerId id, CancellationToken cancellationToken);
    Task<List<DailyTracker>> GetDailyTrackersByParentActivityRuleId(UserId userId, ActivityRuleId activityRuleId, CancellationToken cancellationToken);
}