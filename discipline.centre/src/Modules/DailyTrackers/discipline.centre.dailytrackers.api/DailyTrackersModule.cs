using discipline.centre.shared.abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.dailytrackers.api;

internal sealed class DailyTrackersModule : IModule
{
    internal const string ModuleName = "daily-trackers-module";
    
    public string Name => "DailyTrackers";
    
    public void Register(IServiceCollection services, IConfiguration configuration)
        => services.AddInfrastructure(ModuleName);

    public void Use(WebApplication app)
    {
        app.MapDailyTrackersEndpoints();
    }
}