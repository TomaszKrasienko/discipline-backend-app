using discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.ActivityRules.Clients;

public interface IActivityRulesApiClient
{
    Task<ActivityRuleDto?> GetActivityRuleByIdAsync(ActivityRuleId activityRuleId, UserId userId);
}