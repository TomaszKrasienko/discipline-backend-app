using discipline.application.Behaviours.CQRS.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.infrastructure.Logging.Configuration;

internal static class LoggingServicesConfigurationExtensions
{
    internal static IServiceCollection AddLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerLogDecorator<>));
        return services;
    }
    
    
}