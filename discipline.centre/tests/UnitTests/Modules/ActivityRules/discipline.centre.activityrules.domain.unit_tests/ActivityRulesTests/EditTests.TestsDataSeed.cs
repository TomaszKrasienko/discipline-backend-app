using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class EditTests
{
    public static IEnumerable<object[]> GetValidEditActivityRulesData()
    {
        yield return
        [
            new EditActivityRuleParams("new_test_title", null, Mode.EveryDayMode, null)
        ];

        yield return
        [
            new EditActivityRuleParams("new_test_title", "new_test_title",Mode.CustomMode, [1,2,3])
        ];
    }
    
    public static IEnumerable<object[]> GetInvalidEditActivityRulesData()
    {
        yield return
        [
            new EditActivityRuleParams(string.Empty, null,Mode.CustomMode, [1, 2, 3]),
            "ActivityRule.Details.Title.Empty"
        ];
        
        yield return
        [
            new EditActivityRuleParams(new string('t', 31), null,Mode.CustomMode, [1, 2, 3]),
            "ActivityRule.Details.Title.TooLong"
        ];
        
        yield return
        [
            new EditActivityRuleParams("test_title", null, string.Empty, [1, 2, 3]),
            "ActivityRule.Mode.Empty"
        ];
        
        yield return
        [
            new EditActivityRuleParams("test_title", null, "test_mode", [1, 2, 3]),
            "ActivityRule.Mode.Unavailable"
        ];
        
        yield return
        [
            new EditActivityRuleParams("test_title", null, Mode.CustomMode, [-1, 2, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new EditActivityRuleParams("test_title", null, Mode.CustomMode, [1, 7, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
    }
    
    public static IEnumerable<object[]> GetValidModesForSelectedDays()
    {
        yield return [Mode.CustomMode];
    }
    
    public static IEnumerable<object[]> GetInvalidModesForSelectedDays()
    {
        yield return [Mode.EveryDayMode];
        yield return [Mode.FirstDayOfWeekMode];
        yield return [Mode.LastDayOfWeekMode];
        yield return [Mode.FirstDayOfMonth];
        yield return [Mode.LastDayOfMonthMode];
    }
    
    public static IEnumerable<object[]> GetEditUnchangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title",null, Mode.EveryDayMode, null),
            new EditActivityRuleParams("test_title", null, Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", "test_note", Mode.CustomMode, [1,2,3]),
            new EditActivityRuleParams("test_title", "test_note", Mode.CustomMode, [1,2,3])
        ];
    }

    public static IEnumerable<object[]> GetEditChangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null,Mode.EveryDayMode, null),
            new EditActivityRuleParams("test_title1", null, Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", "test_note",Mode.EveryDayMode, null),
            new EditActivityRuleParams("test_title", null,Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", "test_note",Mode.EveryDayMode, null),
            new EditActivityRuleParams("test_title", "test_note1",Mode.EveryDayMode, null)
        ];      
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null,Mode.EveryDayMode, null),
            new EditActivityRuleParams("test_title", null,Mode.FirstDayOfMonth, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title", null,Mode.CustomMode, [1,2]),
            new EditActivityRuleParams("test_title", null,Mode.CustomMode, [1])
        ];
    }
    
    public sealed record EditActivityRuleParams(string Title, string? Note, string Mode,
        List<int>? SelectedDays = null);
}