using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.infrastructure.Modules;
using discipline.centre.shared.infrastructure.Modules.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ModulesServicesConfigurationExtensions
{
    internal static IServiceCollection AddModule(this IServiceCollection services)
        => services
            .AddSingleton<IModuleTypesTranslator, ModuleTypesTranslator>()
            .AddSingleton<IModuleClient, ModuleClient>()
            .AddSingleton<IModuleRegistry, ModuleRegistry>();
}