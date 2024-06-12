using discipline.application.DTOs;
using discipline.application.DTOs.Mappers;
using discipline.application.Infrastructure.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Features.ActivityRules;

internal static class GetActivityRuleById
{
    internal const string Name = "GetActivityRuleById";
    
    internal static WebApplication MapGetActivityRuleById(this WebApplication app)
    {
        app.MapGet("/activity-rule/{activityRuleId:guid}", async (Guid activityRuleId, DisciplineDbContext dbContext,
            CancellationToken cancellationToken) 
            => (await dbContext
                    .ActivityRules
                    .FirstOrDefaultAsync(x => x.Id.Value == activityRuleId, cancellationToken))?.AsDto()
                )
            .WithName(Name)
            .Produces(StatusCodes.Status200OK, typeof(ActivityRuleDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void));
        return app;
    }
}