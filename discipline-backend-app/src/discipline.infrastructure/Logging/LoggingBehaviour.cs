using discipline.application.Behaviours.CQRS.Commands;
using Microsoft.Extensions.Logging;

namespace discipline.infrastructure.Logging;

internal sealed class CommandHandlerLogDecorator<T>(
    ILogger<ICommandHandler<T>> logger,
    ICommandHandler<T> handler) : ICommandHandler<T> where T : ICommand
{
    public async Task HandleAsync(T command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling command handler for command: {0}", typeof(T));
        try
        {
            await handler.HandleAsync(command, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}