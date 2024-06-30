using discipline.application.Features.ActivityRuleModes;
using discipline.application.Features.ActivityRules;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.application.Features.Configuration.Base.Abstractions;
using discipline.application.Features.Configuration.Base.Internals;
using discipline.application.Features.DailyProductivities.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Features.Configuration;

public static class Extensions
{
    internal static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
        => services.AddDailyProductivityFeatures(configuration);
    
    internal static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        return services;
    }
    
    public static WebApplication MapFeatures(this WebApplication app)
        => app
            .MapActivityRulesFeatures()
            .MapDailyProductiveFeatures()
            .MapGetActivityRuleModes();
}