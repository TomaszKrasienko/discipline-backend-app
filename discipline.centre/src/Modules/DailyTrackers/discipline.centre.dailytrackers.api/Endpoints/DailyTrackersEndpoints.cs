using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.dailytrackers.api;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;
using discipline.centre.dailytrackers.application.DailyTrackers.Queries;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class DailyTrackersEndpoints
{
    private const string DailyTrackersTag = "daily-trackers";
    private const string GetByIdEndpoint = "get-by-id";

    internal static WebApplication MapDailyTrackersEndpoints(this WebApplication app)
    {
        // ReSharper disable once RouteTemplates.RouteParameterConstraintNotResolved
        app.MapPost($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/activities/{{activityRuleId:ulid}}",
            async (Ulid activityRuleId, CancellationToken cancellationToken, IIdentityContext identityContext, 
                ICqrsDispatcher dispatcher, IHttpContextAccessor contextAccessor) =>
            {
                var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
                var activityId = ActivityId.New();
                var userId = identityContext.GetUser();
                
                await dispatcher.HandleAsync(new CreateActivityFromActivityRuleCommand(userId, activityId, stronglyActivityRuleId), cancellationToken);
                contextAccessor.AddResourceIdHeader(activityId.ToString());

                return Results.CreatedAtRoute(GetByIdEndpoint, new { activityId = activityId.ToString() });
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("CreateActivityFromActivityRule")
            .WithTags(DailyTrackersTag)
            .WithDescription("Creates activity from provided activity rule.")
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        app.MapPost($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/activities", async (
            CreateActivityDto dto, CancellationToken cancellationToken, ICqrsDispatcher dispatcher,
            IIdentityContext identityContext, IHttpContextAccessor contextAccessor) =>
            {
                var activityId = ActivityId.New();
                var userId = identityContext.GetUser();
                
                await dispatcher.HandleAsync(dto.MapAsCommand(userId, activityId), cancellationToken);
                contextAccessor.AddResourceIdHeader(activityId.ToString());

                return Results.CreatedAtRoute(GetByIdEndpoint, new { activityId = activityId.ToString() }, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName("CreateActivity")
            .WithTags(DailyTrackersTag)
            .WithDescription("Creates activity.")
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        // ReSharper disable once RouteTemplates.RouteParameterConstraintNotResolved
        app.MapGet($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/activities/{{activityId:ulid}}", async (
            Ulid activityId, CancellationToken cancellationToken, IIdentityContext identityContext, ICqrsDispatcher dispatcher) =>
            {
                var stronglyActivityId = new ActivityId(activityId);
                var userId = identityContext.GetUser();
                
                var result = await dispatcher.SendAsync(new GetActivityByIdQuery(userId, stronglyActivityId), cancellationToken);

                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(ActivityDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(void))
            .WithName(GetByIdEndpoint)
            .WithTags(DailyTrackersTag)
            .WithDescription("Gets activity by its unique identifier.")
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        // ReSharper disable once RouteTemplates.RouteParameterConstraintNotResolved
        app.MapGet($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/{{day:dateonly}}", async (
            DateOnly day, CancellationToken cancellationToken, IIdentityContext identityContext,
            ICqrsDispatcher dispatcher) =>
            {
                var result = await dispatcher.SendAsync(new GetDailyTrackerByDayQuery(identityContext.GetUser(), day), cancellationToken);

                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(ActivityDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(void))
            .WithName("GetDailyTrackerByDay")
            .WithTags(DailyTrackersTag)
            .WithDescription("Gets activity by its day.")
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        app.MapPatch($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/{{dailyTrackerId:ulid}}/activities/{{activityId:ulid}}/check",
            async (Ulid dailyTrackerId, Ulid activityId, CancellationToken cancellationToken, IIdentityContext identityContext, ICqrsDispatcher dispatcher) =>
            {
                var stronglyDailyTrackerId = new DailyTrackerId(dailyTrackerId);
                var stronglyActivityId = new ActivityId(activityId);
                var userId = identityContext.GetUser();
                
                await dispatcher.HandleAsync(new MarkActivityAsCheckedCommand(userId, stronglyDailyTrackerId, stronglyActivityId), cancellationToken);
                
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(void))
            .WithName("MarkActivityAsChecked")
            .WithTags(DailyTrackersTag)
            .WithDescription("Changes checked flag at activity")
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        // ReSharper disable once RouteTemplates.RouteParameterConstraintNotResolved
        app.MapPatch($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/{{dailyTrackerId:ulid}}/activities/{{activityId:ulid}}/stages/{{stageId:ulid}}/check", 
            async (Ulid dailyTrackerId, Ulid activityId, Ulid stageId, CancellationToken cancellationToken, IIdentityContext identityContext, ICqrsDispatcher dispatcher) =>
            {
                var stronglyDailyTrackerId = new DailyTrackerId(dailyTrackerId);
                var stringlyActivityId = new ActivityId(activityId);
                var stronglyStageId = new StageId(stageId);
                
                await dispatcher.HandleAsync(new MarkActivityStageAsCheckedCommand(identityContext.GetUser(), stronglyDailyTrackerId, stringlyActivityId, stronglyStageId), cancellationToken);
                
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(void))
            .WithName("MarkActivityStageAsChecked")
            .WithTags(DailyTrackersTag)
            .WithDescription("Changes checked flag at activity stage")
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        app.MapDelete($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/{{dailyTrackerId:ulid}}/activities/{{activityId:ulid}}/stages/{{stageId:ulid}}",
            async (Ulid dailyTrackerId, Ulid activityId, Ulid stageId, CancellationToken cancellationToken, IIdentityContext identityContext, ICqrsDispatcher dispatcher) =>
            {
                var stronglyDailyTrackerId = new DailyTrackerId(dailyTrackerId);
                var activityStronglyId = new ActivityId(activityId);
                var stronglyStageId = new StageId(stageId);
                var userId = identityContext.GetUser();
                
                await dispatcher.HandleAsync(new DeleteActivityStageCommand(userId, stronglyDailyTrackerId, activityStronglyId, stronglyStageId), cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(void))
            .WithName("DeleteActivityStage")
            .WithTags(DailyTrackersTag)
            .WithDescription("Removes activity stage")
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        return app;
    }
}