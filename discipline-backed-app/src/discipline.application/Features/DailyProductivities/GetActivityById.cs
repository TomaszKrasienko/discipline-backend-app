using System.Runtime.CompilerServices;
using discipline.application.DTOs;
using discipline.application.DTOs.Mappers;
using discipline.application.Infrastructure.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Features.DailyProductivities;

internal static class GetActivityById
{
    internal static WebApplication MapGetActivityById(this WebApplication app)
    {
        app.MapGet("/daily-productive/activities/{activityId:guid}", async (Guid activityId, 
            CancellationToken cancellationToken, DisciplineDbContext dbContext) =>
            {
                var result = (await dbContext
                    .Activities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(activityId), cancellationToken))?.AsDto();
                return result is null ? Results.NoContent() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(ActivityDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .WithName(nameof(GetActivityById))
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets activity by \"ID\""
            });;
        return app;
    }
}