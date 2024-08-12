using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.Progress.Configuration;
using discipline.application.Infrastructure.DAL.Connection;
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
        app.MapGet($"{Extensions.ProgressTag}/data", async (CancellationToken cancellationToken, 
                IDisciplineMongoCollection disciplineMongoCollection) =>
        {
            var result = (await disciplineMongoCollection
                .GetCollection<DailyProductivityDocument>()
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
        .Produces(StatusCodes.Status401Unauthorized, typeof(void))
        .Produces(StatusCodes.Status403Forbidden, typeof(ErrorDto))
        .WithName(nameof(GetProgressData))
        .WithTags(Extensions.ProgressTag)
        .WithOpenApi(operation => new(operation)
        {
            Description = "Gets data progress as day, percent of done activities per day"
        })
         .RequireAuthorization();
        return app;
    }
}


internal static class ProgressCalculator
{
    internal static int Calculate(int activities, int doneActivities)
        => (int)(Math.Round((double)doneActivities * 100 / (double)activities));
}