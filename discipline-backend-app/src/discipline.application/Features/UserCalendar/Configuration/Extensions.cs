using discipline.application.Features.DailyProductivities;
using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.UserCalendar.Configuration;

internal static class Extensions
{
    internal static WebApplication MapUserCalendarFeatures(this WebApplication app)
        => app
            .MapAddImportantDate();
}