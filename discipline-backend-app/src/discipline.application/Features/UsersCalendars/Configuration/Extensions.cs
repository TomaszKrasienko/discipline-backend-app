using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.UsersCalendars.Configuration;

internal static class Extensions
{
    internal static WebApplication MapUserCalendarFeatures(this WebApplication app)
        => app
            .MapAddImportantDate()
            .MapAddCalendarEvent()
            .MapGetEventById()
            .MapBrowseUserCalendar();
}