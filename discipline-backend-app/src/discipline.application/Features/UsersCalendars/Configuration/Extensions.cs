using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.UsersCalendars.Configuration;

internal static class Extensions
{
    internal const string UserCalendarTag = "user-calendar";
    
    internal static WebApplication MapUserCalendarFeatures(this WebApplication app)
        => app
            .MapAddImportantDate()
            .MapAddCalendarEvent()
            .MapAddMeeting()
            .MapGetEventById()
            .MapBrowseUserCalendar();
}