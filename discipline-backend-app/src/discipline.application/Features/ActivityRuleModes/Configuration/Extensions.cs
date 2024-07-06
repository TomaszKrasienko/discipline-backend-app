using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.ActivityRuleModes.Configuration;

internal static class Extensions
{
    public static WebApplication MapActivityRulesModesFeatures(this WebApplication app)
        => app
            .MapGetActivityRuleModes();
}