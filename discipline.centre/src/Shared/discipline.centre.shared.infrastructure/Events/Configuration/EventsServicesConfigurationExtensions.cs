using System.Reflection;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.infrastructure.Events;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class EventsServicesConfigurationExtensions
{
    internal static IServiceCollection AddEvents(this IServiceCollection services, IConfiguration configuration,
        IEnumerable<Assembly> assemblies)
        => services
            .AddRedisBroker(configuration)
            .AddInternalBrokerServices()
            .AddSingleton<IEventProcessor, EventProcessor>()
            .AddSingleton<IEventDispatcher, EventDispatcher>()
            .AddEventHandlers(assemblies);

    private static IServiceCollection AddEventHandlers(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(x => x.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
}