using discipline.centre.shared.abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.activityrules.api;

internal sealed class ActivityRulesModule : IModule
{
    internal const string ModuleName = "activity-rules-module";
    
    public string Name => "ActivityRules";

    public void Register(IServiceCollection services)
        => services.AddInfrastructure(ModuleName);

    public void Use(WebApplication app)
    {
        
    }
}