using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class PostgreSqlActivityRuleRepository(
    DisciplineDbContext dbContext) : IActivityRuleRepository
{
    public async Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
    {
        await dbContext.ActivityRules.AddAsync(activityRule, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
    {
        dbContext.ActivityRules.Update(activityRule);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
    {
        dbContext.ActivityRules.Remove(activityRule);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default)
        => dbContext
            .ActivityRules
            .AnyAsync(x 
                => x.Title == title, cancellationToken);

    public Task<ActivityRule> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext
            .ActivityRules
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
}