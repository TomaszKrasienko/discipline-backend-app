using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.DTOs;
using discipline.application.Features.ActivityRules.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRules;

internal static class GetActivityRuleById
{
    internal static WebApplication MapGetActivityRuleById(this WebApplication app)
    {
        app.MapGet($"/{Extensions.ActivityRulesTag}/{{activityRuleId}}", async (Ulid activityRuleId,  CancellationToken cancellationToken) =>
            {
                // var result = await disciplineMongoCollection
                //     .GetCollection<ActivityRuleDocument>()
                //     .Find(x => x.Id == activityRuleId.ToString())
                //     .FirstOrDefaultAsync(cancellationToken);
                // return result is null ? Results.NoContent() : Results.Ok(result.AsDto());
            })
            .Produces(StatusCodes.Status200OK, typeof(ActivityRuleDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName(nameof(GetActivityRuleById))
            .WithTags(Extensions.ActivityRulesTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets activity rule by \"ID\""
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        return app;
    }
}