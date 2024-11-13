using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;

namespace discipline.centre.activityrules.infrastructure.DAL;

internal sealed class MongoActivityRuleRepository() : IWriteActivityRuleRepository, IReadActivityRuleRepository 
{
    public Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ActivityRule> GetByIdAsync(ActivityRuleId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}