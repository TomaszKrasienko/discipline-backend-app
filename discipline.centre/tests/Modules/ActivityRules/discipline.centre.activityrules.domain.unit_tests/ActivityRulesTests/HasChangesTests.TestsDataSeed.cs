using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class HasChangesTests
{
    public static IEnumerable<object[]> GetHasChangesUnchangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
            new HasChangesParameters("test", Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.CustomMode, [1,2,3]),
            new HasChangesParameters("test", Mode.CustomMode, [1,2,3])
        ];
    }

    public static IEnumerable<object[]> GetHasChangesChangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
            new HasChangesParameters("test1", Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test1", Mode.EveryDayMode, null),
            new HasChangesParameters("test", Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.FirstDayOfMonth, null),
            new HasChangesParameters("test", Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
            new HasChangesParameters("test", Mode.FirstDayOfMonth, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.CustomMode, [1,2]),
            new HasChangesParameters("test", Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.CustomMode, [1,2]),
            new HasChangesParameters("test", Mode.CustomMode, [2,3])
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
            new HasChangesParameters("test", Mode.CustomMode, [1,2])
        ];
    }

    public sealed record HasChangesParameters(string Title, string Mode, List<int>? SelectedDays);
}