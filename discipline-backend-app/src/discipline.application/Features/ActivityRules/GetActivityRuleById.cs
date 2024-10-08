using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace discipline.application.Features.ActivityRules;

internal static class GetActivityRuleById
{
    internal static WebApplication MapGetActivityRuleById(this WebApplication app)
    {
        app.MapGet($"/{Extensions.ActivityRulesTag}/{{activityRuleId:guid}}", async (Guid activityRuleId, 
                IDisciplineMongoCollection disciplineMongoCollection, CancellationToken cancellationToken) =>
            {
                var result = await disciplineMongoCollection
                    .GetCollection<ActivityRuleDocument>()
                    .Find(x => x.Id == activityRuleId)
                    .FirstOrDefaultAsync(cancellationToken);
                return result is null ? Results.NoContent() : Results.Ok(result.AsDto());
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
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);;
        return app;
    }
}