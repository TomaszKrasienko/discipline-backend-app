namespace discipline.centre.dailytrackers.domain.Repositories;

public interface IWriteReadDailyTrackerRepository : IReadDailyTrackerRepository
{
    Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default);
}