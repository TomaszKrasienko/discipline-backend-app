using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace discipline.application.Behaviours;

internal static class LoggingBehaviour
{
    internal static IServiceCollection AddLoggingBehaviour(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerLogDecorator<>));
        return services;
    } 
    
    internal static WebApplicationBuilder UseLoggingBehaviour(this WebApplicationBuilder app)
    {
        app.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}");
        });
        return app;
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