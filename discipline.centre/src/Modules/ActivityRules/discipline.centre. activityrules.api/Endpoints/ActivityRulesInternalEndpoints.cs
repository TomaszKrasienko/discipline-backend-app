using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http;
using discipline.centre.activityrules.api;
using discipline.centre.shared.infrastructure.Auth.Const;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods with mapped endpoints for internal purposes
/// </summary>
internal static class ActivityRulesInternalEndpoints
{
    private const string ActivityRulesInternalTag = "activity-rules-internal";
    
    internal static WebApplication MapActivityRulesInternalEndpoints(this WebApplication app)
    {
        app.MapGet($"/{ActivityRulesModule.ModuleName}/{ActivityRulesInternalTag}/{{userId:ulid}}/{{activityRuleId:ulid}}",
                async (Ulid userId, Ulid activityRuleId, CancellationToken cancellationToken,
                    ICqrsDispatcher dispatcher) =>
                {
                    var stronglyTypedUserId = new UserId(userId);
                    var stronglyTypedActivityRuleId = new ActivityRuleId(activityRuleId);

                    var result = await dispatcher.SendAsync(
                        new GetActivityRuleByIdQuery(stronglyTypedActivityRuleId, stronglyTypedUserId),
                        cancellationToken);
                    return result is null ? Results.NotFound() : Results.Ok(result);
                })
            .RequireAuthorization(policy =>
            {
                policy.AuthenticationSchemes.Add(AuthorizationSchemes.HangfireAuthorizeSchema);
                policy.RequireAuthenticatedUser();
            });
        
        return app;
    }
}