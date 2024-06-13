using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRuleModes;

internal static class GetActivityRuleModes
{
    internal static WebApplication MapGetActivityRuleModes(this WebApplication app)
    {
        app.MapGet("/activity-rule-modes", () 
            => Mode
                .AvailableModes
                .Select(x => new ActivityRuleModeDto()
                {
                    Key = x.Key,
                    Name = x.Value
                }))
            .Produces(StatusCodes.Status200OK);
        return app;
    }
}