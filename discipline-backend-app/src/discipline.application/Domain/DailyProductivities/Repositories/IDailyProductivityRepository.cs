using discipline.application.Domain.DailyProductivities.Entities;

namespace discipline.application.Domain.DailyProductivities.Repositories;

internal interface IDailyProductivityRepository
{
    Task AddAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default);
    Task UpdateAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default);
    Task<DailyProductivity> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default);
    Task<DailyProductivity> GetByActivityId(Guid activityId, CancellationToken cancellationToken = default);
}