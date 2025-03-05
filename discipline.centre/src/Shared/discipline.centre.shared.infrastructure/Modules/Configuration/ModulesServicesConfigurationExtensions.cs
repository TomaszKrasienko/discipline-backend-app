using System.Reflection;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.infrastructure.Modules;
using discipline.centre.shared.infrastructure.Modules.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ModulesServicesConfigurationExtensions
{
    internal static IServiceCollection AddModule(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
        => services
            .AddSingleton<IModuleTypesTranslator, ModuleTypesTranslator>()
            .AddSingleton<IModuleClient, ModuleClient>()
            .AddSingleton<IModuleRegistry, ModuleRegistry>()
            .AddSingleton<IModuleSubscriber, ModuleSubscriber>()
            .AddBroadcastingRegistration(assemblies);

    private static IServiceCollection AddBroadcastingRegistration(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
        => services.AddSingleton<IModuleRegistry>(sp =>
        {
            var registry = new ModuleRegistry();
            var types = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsAssignableTo(typeof(IEvent)) && x.IsClass)
                .ToArray();
            
            var eventDispatcher = sp.GetRequiredService<IEventDispatcher>();
            var eventDispatcherType = eventDispatcher.GetType();

            foreach (var type in types)
            {
                registry.AddBroadcastingRegistration(type, @event => 
                    (Task)eventDispatcherType?.GetMethod(nameof(eventDispatcher.PublishAsync))!
                        .MakeGenericMethod(type)
                        .Invoke(eventDispatcher, [@event])!);
            }

            return registry;

        });
}