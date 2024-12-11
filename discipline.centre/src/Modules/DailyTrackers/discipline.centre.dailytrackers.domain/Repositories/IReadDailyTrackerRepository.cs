using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.Repositories;

public interface IReadDailyTrackerRepository
{
    Task<DailyTracker?> GetDailyTrackerByDayAsync(DateOnly day, UserId userId, CancellationToken cancellationToken = default);
}