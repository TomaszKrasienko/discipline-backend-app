using Microsoft.Extensions.Caching.Memory;

namespace discipline.application.Infrastructure.DAL.UnitOfWork;


internal sealed class PostgresUnitOfWork(
    DisciplineDbContext dbContext) : IUnitOfWork
{
    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await action();
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}