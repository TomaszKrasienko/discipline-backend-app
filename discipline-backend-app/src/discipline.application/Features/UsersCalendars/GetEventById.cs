using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.UsersCalendars;

internal static class GetEventById
{
    internal static WebApplication MapGetEventById(this WebApplication app)
    {
        app.MapGet("user-calendar/events/{eventId:guid}", async (Guid eventId) =>
        {
            throw new NotImplementedException();
        })
        .WithName(nameof(GetEventById));
        return app;
    }
}