using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.activityrules.api;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class ActivityRulesModule : IModule
{
    internal const string ModuleName = "activity-rules-module";
    
    public string Name => "ActivityRules";

    public void Register(IServiceCollection services)
        => services.AddInfrastructure(ModuleName);

    public void Use(WebApplication app)
    {
        app.MapActivityRulesEndpoints();
        app.UseModuleRequest()
            .MapModuleRequest<GetActivityRuleByIdQuery, ActivityRuleDto>("activity-rules/get",(query, sp) 
                => sp.GetRequiredService<ICqrsDispatcher>().SendAsync(query, default));
    }
}