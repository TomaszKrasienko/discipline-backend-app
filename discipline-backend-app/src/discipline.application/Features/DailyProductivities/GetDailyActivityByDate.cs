using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.DTOs;
using discipline.application.Features.DailyProductivities.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.DailyProductivities;

internal static class GetDailyActivityByDate
{
    internal static WebApplication MapGetDailyActivityByDate(this WebApplication app)
    {
        app.MapGet($"/{Extensions.DailyProductivityTag}/{{day:datetime}}",async (DateTime day, CancellationToken cancellationToken) =>
            {
                // var result = (await disciplineMongoCollection
                //     .GetCollection<DailyProductivityDocument>()
                //     .Find(x => x.Day == DateOnly.FromDateTime(day))
                //     .FirstOrDefaultAsync(cancellationToken))?.AsDto();
                // return result is null ? Results.NoContent() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(DailyProductivityDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName(nameof(GetDailyActivityByDate))
            .WithTags(Extensions.DailyProductivityTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets daily discipline by \"Day\""
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        return app;
    }
}