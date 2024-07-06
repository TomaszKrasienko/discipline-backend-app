using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Features.DailyProductivities.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddDailyProductivityFeatures(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddCreateActivityFromRule(configuration);
    
    internal static WebApplication MapDailyProductiveFeatures(this WebApplication app)
        => app
            .MapCreateActivity()
            .MapDeleteActivity()
            .MapChangeActivityCheck()
            .MapGetDailyActivityByDate();
}