using discipline.centre.shared.infrastructure.Configuration;
using discipline.centre.shared.infrastructure.Logging.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace discipline.centre.shared.infrastructure.Logging.Configuration;

internal static class LoggingServicesConfigurationExtensions
{
    internal static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddDistributedTracing();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndBind<JaegerOptions, JaegerOptionsValidator>(configuration)
            .ValidateAndBind<OpenTelemetryOptions, OpenTelemetryOptionsValidator>(configuration);
    
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
}