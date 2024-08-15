using discipline.domain.DailyProductivities.Entities;

namespace discipline.domain.DailyProductivities.Repositories;

public interface IDailyProductivityRepository
{
    Task AddAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default);
    Task UpdateAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default);
    Task<DailyProductivity> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default);
    Task<DailyProductivity> GetByActivityId(Guid activityId, CancellationToken cancellationToken = default);
}