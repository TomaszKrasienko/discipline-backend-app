using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public static class UpdateActivityRuleDtoMapperExtensions
{
    public static UpdateActivityRuleCommand MapAsCommand(this UpdateActivityRuleDto dto, UserId userId, ActivityRuleId activityRuleId)
        => new (userId, activityRuleId, dto.Details, dto.Mode, dto.SelectedDays);
}