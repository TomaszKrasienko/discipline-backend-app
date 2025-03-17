using discipline.centre.shared.abstractions.Attributes;
using discipline.centre.shared.abstractions.CQRS.Commands;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> handler,
    ILogger<ICommandHandler<TCommand>> logger) : ICommandHandler<TCommand> where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling command {0}", command.GetType().Name);
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