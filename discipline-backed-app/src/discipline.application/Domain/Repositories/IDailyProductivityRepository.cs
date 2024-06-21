using discipline.application.Domain.Entities;

namespace discipline.application.Domain.Repositories;

internal interface IDailyProductivityRepository
{
    Task AddAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default);
    Task UpdateAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default);
    Task<DailyProductivity> GetByDateAsync(DateTime day, CancellationToken cancellationToken = default);
}