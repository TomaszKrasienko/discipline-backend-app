using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.Progress.Configuration;

internal static class Extensions
{
    internal static WebApplication MapProgressFeatures(this WebApplication app)
        => app.MapGetProgressData();
}