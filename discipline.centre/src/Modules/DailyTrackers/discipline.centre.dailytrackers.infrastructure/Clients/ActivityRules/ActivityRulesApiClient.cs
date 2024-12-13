using discipline.centre.dailytrackers.application.ActivityRules.Clients;
using discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;
using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.infrastructure.Clients.ActivityRules;

internal sealed class ActivityRulesApiClient(
    IModuleClient moduleClient) : IActivityRulesApiClient
{
    public Task<ActivityRuleDto?> GetActivityRuleByIdAsync(ActivityRuleId activityRuleId, UserId userId)
        => moduleClient.SendAsync<ActivityRuleDto>("activity-rules/get",
            new GetActivityRuleByIdRequestDto(activityRuleId, userId));
}