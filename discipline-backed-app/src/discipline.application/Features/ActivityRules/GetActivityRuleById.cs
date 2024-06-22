using discipline.application.DTOs;
using discipline.application.DTOs.Mappers;
using discipline.application.Infrastructure.DAL;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace discipline.application.Features.ActivityRules;

internal static class GetActivityRuleById
{
    internal static WebApplication MapGetActivityRuleById(this WebApplication app)
    {
        app.MapGet("/activity-rules/{activityRuleId:guid}", async (Guid activityRuleId, IMongoDatabase mongoDatabase,
            CancellationToken cancellationToken) =>
            {
                var collection = mongoDatabase.GetCollection<ActivityRuleDocument>("ActivityRules");
                var result = await collection
                    .Find(x => x.Id == activityRuleId)
                    .FirstOrDefaultAsync(cancellationToken);
                return result is null ? Results.NoContent() : Results.Ok(result.AsDto());
            })
            .Produces(StatusCodes.Status200OK, typeof(ActivityRuleDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .WithName(nameof(GetActivityRuleById))
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets activity rule by \"ID\""
            });
        return app;
    }
}