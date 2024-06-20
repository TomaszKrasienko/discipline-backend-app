using discipline.application.DTOs;
using discipline.application.DTOs.Mappers;
using discipline.application.Infrastructure.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Features.ActivityRules;

internal static class GetActivityRuleById
{
    internal static WebApplication MapGetActivityRuleById(this WebApplication app)
    {
        app.MapGet("/activity-rules/{activityRuleId:guid}", async (Guid activityRuleId, DisciplineDbContext dbContext,
            CancellationToken cancellationToken) =>
            {
                var result = (await dbContext
                    .ActivityRules
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(activityRuleId), cancellationToken))?.AsDto();
                return result is null ? Results.NoContent() : Results.Ok(result);
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