using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable All
namespace discipline.centre.activityrules.api.Endpoints;

internal static class ActivityRulesEndpoints
{
    private const string ActivityRulesTag = "activity-rules";
    private const string GetActivityRuleById = "GetActivityRuleById";
    
    internal static WebApplication MapActivityRulesEndpoints(this WebApplication app)
    {
        app.MapPost($"/{ActivityRulesModule.ModuleName}/{ActivityRulesTag}", async (CreateActivityRuleCommand command, IHttpContextAccessor httpContext, 
                ICqrsDispatcher dispatcher, CancellationToken cancellationToken, IIdentityContext identityContext) => 
            {
                var activityRuleId = ActivityRuleId.New();
                var userId = identityContext.GetUser();
                
                await dispatcher.HandleAsync(command with { Id = activityRuleId, UserId = userId! }, cancellationToken);
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

        app.MapPut($"/{ActivityRulesModule.ModuleName}/{ActivityRulesTag}/{{activityRuleId:ulid}}", async (Ulid activityRuleId, UpdateActivityRuleDto dto,
            CancellationToken cancellationToken, ICqrsDispatcher dispatcher) =>
        {
            var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
            await dispatcher.HandleAsync(dto.MapAsCommand(stronglyActivityRuleId), cancellationToken);

            return Results.NoContent();
        });

        app.MapGet($"/{ActivityRulesModule.ModuleName}/{ActivityRulesTag}/{{activityRuleId:ulid}}", async (Ulid activityRuleId,
                CancellationToken cancellationToken, ICqrsDispatcher dispatcher, IIdentityContext identityContext) =>
            {   
                var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
                var result = await dispatcher.SendAsync(new GetActivityRuleByIdQuery(stronglyActivityRuleId,
                    identityContext.GetUser()), cancellationToken);
                
                return Results.Ok(result);
            })            
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
            .WithName(GetActivityRuleById)
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
               Description = "Get activity rule by id" 
            })
            .RequireAuthorization();

        return app;
    }
}