namespace discipline.centre.dailytrackers.domain.Repositories;

/// <summary>
/// The <see cref="DailyTracker"/> interface to perform database <c>write</c> and <c>read</c> operations
/// </summary>
public interface IReadWriteDailyTrackerRepository : IReadDailyTrackerRepository
{
    /// <summary>
    /// Asynchronously adds a new <see cref="DailyTracker"/> to database
    /// </summary>
    /// <param name="dailyTracker">The Daily tracker to be added.</param>
    /// <param name="cancellationToken">Token for monitoring cancellation requests.</param>
    Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously updates an existing <see cref="DailyTracker"/> in databse
    /// </summary>
    /// <param name="dailyTracker">The Daily tracker to be updated.</param>
    /// <param name="cancellationToken">Token for monitoring cancellation requests.</param>
    Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<DailyTracker> dailyTrackers, CancellationToken cancellationToken = default);
}