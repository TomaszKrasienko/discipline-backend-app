using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;

/// <summary>
/// Data Transfer Object for GetActivityRuleById request. Used in <see cref="IActivityRulesApiClient"/>
/// </summary>
/// <param name="activityRuleId">Identifier of activity rule</param>
/// <param name="userId">Identifier of user</param>
public sealed record GetActivityRuleByIdRequestDto(ActivityRuleId activityRuleId, UserId userId);