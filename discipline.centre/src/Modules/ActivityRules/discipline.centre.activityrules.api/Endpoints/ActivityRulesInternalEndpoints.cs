using System.Globalization;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http;
using discipline.centre.activityrules.api;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
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
            async (Ulid userId, Ulid activityRuleId, CancellationToken cancellationToken, ICqrsDispatcher dispatcher) =>
            {
                var stronglyUserId = new UserId(userId);
                var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);

                var result = await dispatcher.SendAsync(new GetActivityRuleByIdQuery(stronglyUserId, stronglyActivityRuleId), cancellationToken);
                
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(ActivityRuleDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(void))
            .WithName("GetActivityRuleForUserById")
            .WithTags(ActivityRulesInternalTag)
            .RequireAuthorization(policy =>
            {
                policy.AuthenticationSchemes.Add(AuthorizationSchemes.HangfireAuthorizeSchema);
                policy.RequireAuthenticatedUser();
            });

        app.MapGet($"/{ActivityRulesModule.ModuleName}/{ActivityRulesInternalTag}/modes", async (
                DateTime day, ICqrsDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                var result = await dispatcher.SendAsync(new GetActiveModesByDayQuery(DateOnly.FromDateTime(day)), cancellationToken);
                
                return Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(ActiveModesDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .WithName("GetActiveModesByDay")
            .WithTags(ActivityRulesInternalTag)
            .RequireAuthorization(policy =>
            {
                policy.AuthenticationSchemes.Add(AuthorizationSchemes.HangfireAuthorizeSchema);
                policy.RequireAuthenticatedUser();
            });
            
        return app;
    }
}