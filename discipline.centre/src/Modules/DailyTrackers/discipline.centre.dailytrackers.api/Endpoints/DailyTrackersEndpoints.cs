using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.dailytrackers.api;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class DailyTrackersEndpoints
{
    private const string DailyTrackersTag = "daily-trackers";

    internal static WebApplication MapDailyTrackersEndpoints(this WebApplication app)
    {
        app.MapPost($"api/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/activities/{{activityRuleId:ulid}}",
            async (Ulid activityRuleId, CancellationToken cancellationToken, IIdentityContext identityContext, 
                ICqrsDispatcher dispatcher, IHttpContextAccessor contextAccessor) =>
            {
                var stronglyTypedActivityRuleId = new ActivityRuleId(activityRuleId);
                var activityId = ActivityId.New();
                
                await dispatcher.HandleAsync(new CreateActivityFromActivityRuleCommand(activityId, stronglyTypedActivityRuleId,
                    identityContext.GetUser()), cancellationToken);
                
                contextAccessor.AddResourceIdHeader(activityId.ToString());

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("CreateActivityFromActivityRule")
            .WithTags(DailyTrackersTag)
            .WithDescription("Creates activity from provided activity rule.");
        
        return app;
    }
}