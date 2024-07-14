using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace discipline.application.Features.UsersCalendars;

internal static class BrowseUserCalendar
{
    internal static WebApplication MapBrowseUserCalendar(this WebApplication app)
    {
        app.MapGet("user-calendar/{day:datetime}", async (DateOnly day,
            IDisciplineMongoCollection disciplineMongoCollection) =>
            {
                var result = await disciplineMongoCollection
                    .GetCollection<UserCalendarDocument>()
                    .Find(x => x.Day == day)
                    .FirstOrDefaultAsync();
                return result is null ? Results.NoContent() : Results.Ok(result.AsDto());
            })
            .Produces(StatusCodes.Status200OK, typeof(UserCalendarDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .WithName(nameof(BrowseUserCalendar))
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets user calendar by \"Day\""
            });
        return app;
    }
}