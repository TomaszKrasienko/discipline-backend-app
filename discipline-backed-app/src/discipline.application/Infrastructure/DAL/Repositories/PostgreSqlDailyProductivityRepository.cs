using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class PostgreSqlDailyProductivityRepository(
    DisciplineDbContext dbContext) : IDailyProductivityRepository
{
    public async Task AddAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default)
    {
        await dbContext.DailyProductivity.AddAsync(dailyProductivity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DailyProductivity dailyProductivity, CancellationToken cancellationToken = default)
    {
        dbContext.DailyProductivity.Update(dailyProductivity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<DailyProductivity> GetByDateAsync(DateTime day, CancellationToken cancellationToken = default)
        => dbContext
            .DailyProductivity
            .Include(x => x.Activities)
            .FirstOrDefaultAsync(x => x.Day == day, cancellationToken);
}