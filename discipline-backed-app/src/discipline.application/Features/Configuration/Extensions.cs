using discipline.application.Features.ActivityRules;
using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.Configuration;

public static class Extensions
{
    public static WebApplication MapFeatures(this WebApplication app)
        => app.MapCreateActivityRule();
}