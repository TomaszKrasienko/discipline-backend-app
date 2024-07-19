using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.ActivityRuleModes.Configuration;

internal static class Extensions
{
    internal const string ActivityRuleModesTag = "activity-rule-modes";
    public static WebApplication MapActivityRulesModesFeatures(this WebApplication app)
        => app
            .MapGetActivityRuleModes();
}