using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace discipline.application.Features.Progress;

internal static class GetProgressData
{
    internal static WebApplication MapGetProgressData(this WebApplication app)
    {
        app.MapGet("progress/data", async (CancellationToken cancellationToken, IMongoDatabase mongoDatabase) =>
        {
            var result = (await mongoDatabase
                .GetCollection<DailyProductivityDocument>(MongoDailyProductivityRepository.CollectionName)
                .Find(_ => true)
                .ToListAsync(cancellationToken));

            if (!result.Any())
            {
                return Results.NoContent();
            }
            
            return Results.Ok(result.Select(x => new ProgressDataDto()
            {
                Day = x.Day,
                Percent = ProgressCalculator.Calculate(
                    x.Activities.Count(),
                    x.Activities.Where(y => y.IsChecked).ToList().Count)
            }).OrderBy(x => x.Day));
        }) 
        .Produces(StatusCodes.Status200OK, typeof(IEnumerable<ProgressDataDto>))
        .Produces(StatusCodes.Status204NoContent, typeof(void))
        .WithName(nameof(GetProgressData))
        .WithOpenApi(operation => new(operation)
        {
            Description = "Gets data progress as day, percent of done activities per day"
        });;
        return app;
    }
}


internal static class ProgressCalculator
{
    internal static int Calculate(int activities, int doneActivities)
        => (int)(Math.Round((double)doneActivities * 100 / (double)activities));
}