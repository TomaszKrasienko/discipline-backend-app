using discipline.application.Features.ActivityRuleModes;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.application.Features.DailyProductivities.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Features.Configuration;

public static class Extensions
{
    internal static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
        => services.AddDailyProductivityFeatures(configuration);
    
    public static WebApplication MapFeatures(this WebApplication app)
        => app
            .MapActivityRulesFeatures()
            .MapDailyProductiveFeatures()
            .MapGetActivityRuleModes();
}