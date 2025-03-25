using discipline.centre.shared.abstractions.Attributes;
using discipline.centre.shared.abstractions.CQRS.Queries;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    ILogger<IQueryHandler<TQuery, TResult>> logger) : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Handling query {0}", query.GetType().Name);
        try
        {
            return await queryHandler.HandleAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}