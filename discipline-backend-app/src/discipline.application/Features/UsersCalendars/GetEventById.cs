using discipline.application.Features.UsersCalendars.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class GetEventById
{
    internal static WebApplication MapGetEventById(this WebApplication app)
    {
        app.MapGet($"{Extensions.UserCalendarTag}/events/{{eventId:guid}}", async (Guid eventId) =>
        {
            throw new NotImplementedException();
        })
        .WithName(nameof(GetEventById))
        .WithTags(Extensions.UserCalendarTag);
        return app;
    }
}