using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class HasChangesTests
{
    public static IEnumerable<object[]> GetHasChangesUnchangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null, Mode.EveryDayMode, null),
            new HasChangesParameters("test_title", null, Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", "test_note",Mode.CustomMode, [1,2,3]),
            new HasChangesParameters("test_title", "test_note", Mode.CustomMode, [1,2,3])
        ];
    }

    public static IEnumerable<object[]> GetHasChangesChangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null, Mode.EveryDayMode, null),
            new HasChangesParameters("test_title1", null,Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null, Mode.EveryDayMode, null),
            new HasChangesParameters("test_title", "test_note", Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", "test_note", Mode.EveryDayMode, null),
            new HasChangesParameters("test_title", "test_note1", Mode.EveryDayMode, null)
        ];
                
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", "test_note", Mode.EveryDayMode, null),
            new HasChangesParameters("test_title", "test_note", Mode.FirstDayOfMonth, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null,Mode.CustomMode, [1,2]),
            new HasChangesParameters("test_title", null,Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null,Mode.CustomMode, [1,2]),
            new HasChangesParameters("test_title", null,Mode.CustomMode, [2,3])
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null,Mode.EveryDayMode, null),
            new HasChangesParameters("test_title", null,Mode.CustomMode, [1,2])
        ];
    }

    public sealed record HasChangesParameters(string Title, string? Note, string Mode, List<int>? SelectedDays);
}