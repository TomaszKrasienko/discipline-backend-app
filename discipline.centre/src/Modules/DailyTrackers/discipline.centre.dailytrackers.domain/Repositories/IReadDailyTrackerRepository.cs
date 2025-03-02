using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.Repositories;

/// <summary>
/// Interface to perform database <c>read</c> operations on <see cref="DailyTracker"/>
/// </summary>
public interface IReadDailyTrackerRepository
{
    /// <summary>
    /// Asynchronously retrieves <see cref="DailyTracker"/> based on specific parameters.
    /// </summary>
    /// <param name="day">The <c>date</c> for which <see cref="DailyTracker"/> is requested</param>
    /// <param name="userId">The <c>userId</c> for which <see cref="DailyTracker"/> is requested</param>
    /// <param name="cancellationToken">Token for monitoring cancellation requests.</param>
    /// <returns>The instance of <see cref="DailyTracker"/> if exists, otherwise null</returns>
    Task<DailyTracker?> GetDailyTrackerByDayAsync(DateOnly day, UserId userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously retrieves <see cref="DailyTracker"/> based on specific parameters.
    /// </summary>
    /// <param name="id">The identifier for which <see cref="DailyTracker"/> is requested.</param>
    /// <param name="userId">The <c>User</c> identifier for which <see cref="DailyTracker"/> is requested.</param>
    /// <param name="cancellationToken">Token for monitoring cancellation requests.</param>
    /// <returns>The instance of <see cref="DailyTracker"/> if exists, otherwise null</returns>
    Task<DailyTracker?> GetDailyTrackerByIdAsync(DailyTrackerId id, UserId userId, CancellationToken cancellationToken = default);
    
    Task<List<DailyTracker>> GetDailyTrackersByParentActivityRuleId(ActivityRuleId activityRuleId, UserId userId, CancellationToken cancellationToken = default);
}