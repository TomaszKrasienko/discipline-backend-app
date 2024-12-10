using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.ActivityRules.Clients;

public interface IActivityRulesApiClient
{
    Task<ActivityRuleId> GetActivityRuleByIdAsync(ActivityRuleId activityRuleId);
}