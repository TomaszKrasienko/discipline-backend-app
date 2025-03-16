using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.infrastructure.Events.Brokers.Redis.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.dailytrackers.api;

internal sealed class DailyTrackersModule : IModule
{
    internal const string ModuleName = "daily-trackers-module";
    public string Name => "DailyTrackers";
    
    public void Register(IServiceCollection services, IConfiguration configuration)
        => services
            .AddInfrastructure()
            .AddRedisConsumerService<CreateActivityFromActivityRuleCommand>();

    public void Use(WebApplication app)
    {
        app.MapDailyTrackersEndpoints();
    }
}