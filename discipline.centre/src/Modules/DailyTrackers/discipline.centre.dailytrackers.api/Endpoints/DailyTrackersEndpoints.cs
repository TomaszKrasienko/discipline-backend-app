using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.dailytrackers.api;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class DailyTrackersEndpoints
{
    private const string DailyTrackersTag = "daily-trackers";

    internal static WebApplication MapDailyTrackersEndpoints(this WebApplication app)
    {
        app.MapPost($"/{DailyTrackersModule.ModuleName}/{DailyTrackersTag}/{{activityRuleId:ulid}}",
            async (Ulid activityRuleId, CancellationToken cancellationToken,
                IIdentityContext identityContext, ICqrsDispatcher dispatcher) =>
            {
                var stronglyTypedActivityRuleId = new ActivityRuleId(activityRuleId);

                await dispatcher.HandleAsync(new CreateActivityFromActivityRuleCommand(stronglyTypedActivityRuleId,
                    identityContext.GetUser()), cancellationToken);
            });
        
        return app;
    }
}