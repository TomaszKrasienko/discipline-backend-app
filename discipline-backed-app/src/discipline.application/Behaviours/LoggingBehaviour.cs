using discipline.application.Features.Configuration.Base.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace discipline.application.Behaviours;

internal static class LoggingBehaviour
{
    internal static IServiceCollection AddLoggingBehaviour(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerLogDecorator<>));
        return services;
    } 
}

internal sealed class CommandHandlerLogDecorator<T>(
    ILogger<ICommandHandler<T>> logger,
    ICommandHandler<T> handler) : ICommandHandler<T> where T : ICommand
{
    public async Task HandleAsync(T command, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling command handler for command: {typeof(T)}");
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