using discipline.application.Domain.Entities;

namespace discipline.application.Domain.Repositories;

internal interface IActivityRuleRepository
{
    Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default);
    Task<ActivityRule> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}