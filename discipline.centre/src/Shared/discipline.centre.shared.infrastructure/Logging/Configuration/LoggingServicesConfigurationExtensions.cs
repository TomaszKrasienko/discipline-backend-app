using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.infrastructure.Configuration;
using discipline.centre.shared.infrastructure.Logging.Configuration.Options;
using discipline.centre.shared.infrastructure.Logging.Decorators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace discipline.centre.shared.infrastructure.Logging.Configuration;

internal static class LoggingServicesConfigurationExtensions
{
    internal static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<UserContextEnrichmentMiddleware>()
            .AddOptions(configuration)
            .AddDistributedTracing()
            .AddLoggingDecorators();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndBind<JaegerOptions, JaegerOptionsValidator>(configuration)
            .ValidateAndBind<OpenTelemetryOptions, OpenTelemetryOptionsValidator>(configuration)
            .ValidateAndBind<SeqOptions, SeqOptionsValidator>(configuration);
    
    private static IServiceCollection AddDistributedTracing(this IServiceCollection services)
    {
        var appOptions = services.GetOptions<AppOptions>();
        var jaegerOptions = services.GetOptions<JaegerOptions>();
        var openTelemetryOptions = services.GetOptions<OpenTelemetryOptions>();
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource
                => resource.AddService(appOptions.Name!))
            .WithTracing(tracing 
                => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource(openTelemetryOptions.InternalSourceName)
                    .AddOtlpExporter(options => options.Endpoint = new Uri(jaegerOptions.Endpoint!)));
        
        return services;
    }

    private static IServiceCollection AddLoggingDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));

        return services;
    }
}