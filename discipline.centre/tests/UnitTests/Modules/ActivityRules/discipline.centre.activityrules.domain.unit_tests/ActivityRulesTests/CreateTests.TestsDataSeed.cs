using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class CreateTests
{
    public static IEnumerable<object[]> GetValidCreateActivityRulesData()
    {
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                Mode.EveryDayMode, null)
        ];

        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                Mode.CustomMode, [1,2,3])
        ];
    }
    

    public static IEnumerable<object[]> GetInvalidCreateActivityRulesData()
    {
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), string.Empty,
                Mode.CustomMode, [1, 2, 3]),
            "ActivityRule.Title.Empty"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                string.Empty, [1, 2, 3]),
            "ActivityRule.Mode.Empty"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                "test_mode", [1, 2, 3]),
            "ActivityRule.Mode.Unavailable"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), string.Empty,
                Mode.CustomMode, [-1, 2, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), string.Empty,
                Mode.CustomMode, [1, 7, 3]),
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
    
    public sealed record ActivityRuleParams(ActivityRuleId? Id, UserId? UserId, string Title, string Mode,
        List<int>? SelectedDays = null);
}