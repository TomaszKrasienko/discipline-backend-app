using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class EditTests
{
    public static IEnumerable<object[]> GetValidEditActivityRulesData()
    {
        yield return
        [
            new ActivityRuleParams(null, null, "test_title", Mode.EveryDayMode, null)
        ];

        yield return
        [
            new ActivityRuleParams(null, null, "test_title", Mode.CustomMode, [1,2,3])
        ];
    }
    
    public static IEnumerable<object[]> GetInvalidEditActivityRulesData()
    {
        yield return
        [
            new ActivityRuleParams(null, null, string.Empty,
                Mode.CustomMode, [1, 2, 3]),
            "ActivityRule.Title.Empty"
        ];
        
        yield return
        [
            new ActivityRuleParams(null, null, "test_title",
                string.Empty, [1, 2, 3]),
            "ActivityRule.Mode.Empty"
        ];
        
        yield return
        [
            new ActivityRuleParams(null, null, "test_title",
                "test_mode", [1, 2, 3]),
            "ActivityRule.Mode.Unavailable"
        ];
        
        yield return
        [
            new ActivityRuleParams(null, null, "test_title",
                Mode.CustomMode, [-1, 2, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new ActivityRuleParams(null, null, "test_title",
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