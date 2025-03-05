using discipline.centre.shared.abstractions.Modules;

namespace discipline.centre.bootstrappers.api;

internal static class ModuleServiceRegistry
{
    internal static IServiceCollection AddModulesConfiguration(this IServiceCollection services, 
        IList<IModule> modules, IConfiguration configuration)
    {
        foreach (var module in modules)
        {
            module.Register(services, configuration);
        }
        return services;
    }
    
    internal static WebApplication UseModulesConfiguration(this WebApplication app, 
        IList<IModule> modules)
    {
        foreach (var module in modules)
        {
            module.Use(app);
        }
        return app;
    }
}