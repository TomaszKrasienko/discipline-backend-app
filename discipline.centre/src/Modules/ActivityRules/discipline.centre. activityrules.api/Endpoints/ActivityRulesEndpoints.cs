using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.centre.activityrules.api.Endpoints;

internal static class ActivityRulesEndpoints
{
    private const string ActivityRulesTag = "activity-rules";
    
    internal static WebApplication MapActivityRulesEndpoints(this WebApplication app)
    {
        app.MapPost($"/{ActivityRulesModule.ModuleName}/{ActivityRulesTag}", async (CreateActivityRuleCommand command, HttpContextAccessor httpContext, 
                ICqrsDispatcher dispatcher, CancellationToken cancellationToken, IIdentityContext identityContext) => 
            {
                var activityRuleId = ActivityRuleId.New();
                var userId = identityContext.UserId;
                await dispatcher.HandleAsync(command with { Id = activityRuleId, UserId = userId }, cancellationToken);
                httpContext.AddResourceIdHeader(activityRuleId.ToString());
                return Results.CreatedAtRoute(nameof(GetActivityRuleById), new {activityRuleId = activityRuleId}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("CreateActivityRule")
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds activity rule"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        app.MapGet($"/{ActivityRulesModule.ModuleName}/{ActivityRulesTag}")
    }
}