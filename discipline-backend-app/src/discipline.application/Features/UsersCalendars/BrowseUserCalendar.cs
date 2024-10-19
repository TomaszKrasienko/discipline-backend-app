using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.UsersCalendars.Configuration;
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
        app.MapGet($"{Extensions.UserCalendarTag}/{{day:datetime}}", async (DateOnly day,
            IIdentityContext identityContext, IDisciplineMongoCollection disciplineMongoCollection,
            CancellationToken cancellationToken) =>
            {
                var result = await disciplineMongoCollection
                    .GetCollection<UserCalendarDocument>()
                    .Find(x => x.Day == day && x.UserId == identityContext.UserId.Value)
                    .FirstOrDefaultAsync(cancellationToken);
                return result is null ? Results.NoContent() : Results.Ok(result.AsDto());
            })
            .Produces(StatusCodes.Status200OK, typeof(UserCalendarDto))
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName(nameof(BrowseUserCalendar))
            .WithTags(Extensions.UserCalendarTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets user calendar by \"Day\""
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);;
        return app;
    }
}