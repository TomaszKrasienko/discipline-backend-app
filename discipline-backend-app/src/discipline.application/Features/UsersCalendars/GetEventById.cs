using discipline.application.Features.UsersCalendars.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class GetEventById
{
    internal static WebApplication MapGetEventById(this WebApplication app)
    {
        app.MapGet($"{Extensions.UserCalendarTag}/events/{{eventId}}", (Ulid eventId) =>
                {
                    return Results.Ok();
                })
            .WithName(nameof(GetEventById));
        return app;
    }
}