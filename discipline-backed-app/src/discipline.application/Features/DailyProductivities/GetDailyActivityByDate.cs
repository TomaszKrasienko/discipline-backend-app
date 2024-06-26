using discipline.application.DTOs;
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
        app.MapGet("/daily-productivity/{day:datetime}",async (DateTime day, CancellationToken cancellationToken,
                IMongoDatabase mongoDatabase) =>
            {
                var result = (await mongoDatabase
                    .GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
                    .Find(x => x.Day == DateOnly.FromDateTime(day))
                    .FirstOrDefaultAsync(cancellationToken))?.AsDto();
                return result is null ? Results.NoContent() : Results.Ok(result);
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