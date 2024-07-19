using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.Progress.Configuration;

internal static class Extensions
{
    internal const string ProgressTag = "progress";
    
    internal static WebApplication MapProgressFeatures(this WebApplication app)
        => app.MapGetProgressData();
}