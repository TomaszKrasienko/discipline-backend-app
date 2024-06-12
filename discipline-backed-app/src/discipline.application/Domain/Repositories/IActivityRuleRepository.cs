using discipline.application.Domain.Entities;

namespace discipline.application.Domain.Repositories;

internal interface IActivityRuleRepository
{
    Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
}