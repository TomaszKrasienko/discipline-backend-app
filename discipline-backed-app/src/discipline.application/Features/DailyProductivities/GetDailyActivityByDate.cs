using discipline.application.DTOs;
using discipline.application.DTOs.Mappers;
using discipline.application.Infrastructure.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Features.DailyProductivities;

internal static class GetDailyActivityByDate
{
    internal static WebApplication MapGetDailyActivityByDate(this WebApplication app)
    {
        app.MapGet("/daily-productivity/{day:datetime}",async (DateTime day, CancellationToken cancellationToken,
                DisciplineDbContext dbContext) =>
            {
                var result = await dbContext
                    .DailyProductivity
                    .Include(x => x.Activities)
                    .ToListAsync(cancellationToken);
                var mapped = result?
                    .FirstOrDefault(x => x.Day == day)?
                    .AsDto();
                return mapped is null ? Results.NoContent() : Results.Ok(mapped);
            })
            .Produces(StatusCodes.Status200OK, typeof(DailyProductivityDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .WithName(nameof(GetDailyActivityByDate))
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets daily discipline by \"Day\""
            });
        return app;
    }
}