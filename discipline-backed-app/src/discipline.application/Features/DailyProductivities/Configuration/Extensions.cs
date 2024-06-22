using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.DailyProductivities.Configuration;

internal static class Extensions
{
    internal static WebApplication MapDailyProductiveFeatures(this WebApplication app)
        => app
            .MapCreateActivity()
            .MapGetDailyActivityByDate();
}