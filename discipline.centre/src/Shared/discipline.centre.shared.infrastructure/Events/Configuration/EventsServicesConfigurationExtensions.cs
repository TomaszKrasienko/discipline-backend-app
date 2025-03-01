using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.infrastructure.Events;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Configuration;
using Microsoft.Extensions.Configuration;
using RedisBrokerOptions = discipline.centre.shared.infrastructure.Events.Brokers.Redis.Configuration.RedisBrokerOptions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class EventsServicesConfigurationExtensions
{
    internal static IServiceCollection AddEvents(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddRedisBroker(configuration)
            .AddInternalBrokerServices()
            .AddSingleton<IEventProcessor, EventProcessor>();
}