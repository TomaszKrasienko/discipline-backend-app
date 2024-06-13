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

    public Task<bool> ExistsAsync(string title, CancellationToken cancellationToken = default)
        => dbContext
            .ActivityRules
            .AnyAsync(x 
                => x.Title == title, cancellationToken);
}