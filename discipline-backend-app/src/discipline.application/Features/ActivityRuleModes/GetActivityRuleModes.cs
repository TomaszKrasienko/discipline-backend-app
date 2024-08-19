using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.ActivityRuleModes.Configuration;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRuleModes;

internal static class GetActivityRuleModes
{
    //TODO: Testy integracyjne + dodanie uwierzytelniania
    internal static WebApplication MapGetActivityRuleModes(this WebApplication app)
    {
        app.MapGet($"/{Extensions.ActivityRuleModesTag}", () 
            => Mode
                .AvailableModes
                .Select(x => new ActivityRuleModeDto()
                {
                    Key = x.Key,
                    Name = x.Value
                }))
            .Produces(StatusCodes.Status200OK)
            .WithName(nameof(GetActivityRuleModes))
            .WithTags(Extensions.ActivityRuleModesTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets activity rule modes"
            })
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);
        return app;
    }
}