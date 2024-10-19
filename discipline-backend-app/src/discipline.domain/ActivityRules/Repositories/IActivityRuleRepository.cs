using discipline.domain.ActivityRules.Entities;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.ActivityRules.Repositories;

public interface IActivityRuleRepository
{
    Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default);
    Task<ActivityRule> GetByIdAsync(ActivityRuleId id, CancellationToken cancellationToken = default);
    Task<List<ActivityRule>> BrowseAsync(CancellationToken cancellationToken = default);
}