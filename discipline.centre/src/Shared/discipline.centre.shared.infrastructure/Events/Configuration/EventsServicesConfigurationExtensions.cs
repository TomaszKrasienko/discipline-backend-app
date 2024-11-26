using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.infrastructure.Events;
using discipline.centre.shared.infrastructure.Events.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class EventsServicesConfigurationExtensions
{
    internal static IServiceCollection AddEvents(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddSingleton<IEventProcessor, EventProcessor>();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.ValidateAndBind<RedisBrokerOptions, RedisBrokerOptionsValidator>(configuration);
}