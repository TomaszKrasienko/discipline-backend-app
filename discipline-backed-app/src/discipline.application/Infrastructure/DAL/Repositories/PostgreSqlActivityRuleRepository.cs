using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class PostgreSqlActivityRuleRepository : IActivityRuleRepository
{
    public Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}