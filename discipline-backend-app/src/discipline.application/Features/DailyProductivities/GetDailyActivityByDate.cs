using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.DailyProductivities.Configuration;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace discipline.application.Features.DailyProductivities;

internal static class GetDailyActivityByDate
{
    internal static WebApplication MapGetDailyActivityByDate(this WebApplication app)
    {
        app.MapGet($"/{Extensions.DailyProductivityTag}/{{day:datetime}}",async (DateTime day, CancellationToken cancellationToken,
                IDisciplineMongoCollection disciplineMongoCollection) =>
            {
                var result = (await disciplineMongoCollection
                    .GetCollection<DailyProductivityDocument>()
                    .Find(x => x.Day == DateOnly.FromDateTime(day))
                    .FirstOrDefaultAsync(cancellationToken))?.AsDto();
                return result is null ? Results.NoContent() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(DailyProductivityDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(ErrorDto))
            .WithName(nameof(GetDailyActivityByDate))
            .WithTags(Extensions.DailyProductivityTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets daily discipline by \"Day\""
            })
            .RequireAuthorization();
        return app;
    }
}