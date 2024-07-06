namespace discipline.application.Infrastructure.DAL.UnitOfWork;

internal interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
}