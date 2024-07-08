using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.ActivityRules.Configuration;

internal static class Extensions
{
    public static WebApplication MapActivityRulesFeatures(this WebApplication app)
        => app
            .MapCreateActivityRule()
            .MapEditActivityRule()
            .MapDeleteActivityRule()
            .MapGetActivityRuleById()
            .MapBrowseActivityRules();
}