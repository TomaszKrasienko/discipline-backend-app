// using discipline.centre.activityrules.domain.ValueObjects;
// using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
// using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
//
// namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;
//
// public partial class EditTests
// {
//     public static IEnumerable<object[]> GetValidEditActivityRulesData()
//     {
//         yield return
//         [
//             new EditActivityRuleParams("test_title", Mode.EveryDayMode, null)
//         ];
//
//         yield return
//         [
//             new EditActivityRuleParams("test_title", Mode.CustomMode, [1,2,3])
//         ];
//     }
//     
//     public static IEnumerable<object[]> GetInvalidEditActivityRulesData()
//     {
//         yield return
//         [
//             new EditActivityRuleParams(string.Empty, Mode.CustomMode, [1, 2, 3]),
//             "ActivityRule.Title.Empty"
//         ];
//         
//         yield return
//         [
//             new EditActivityRuleParams("test_title", string.Empty, [1, 2, 3]),
//             "ActivityRule.Mode.Empty"
//         ];
//         
//         yield return
//         [
//             new EditActivityRuleParams("test_title", "test_mode", [1, 2, 3]),
//             "ActivityRule.Mode.Unavailable"
//         ];
//         
//         yield return
//         [
//             new EditActivityRuleParams("test_title", Mode.CustomMode, [-1, 2, 3]),
//             "ActivityRule.SelectedDay.OutOfRange"
//         ];
//         
//         yield return
//         [
//             new EditActivityRuleParams("test_title", Mode.CustomMode, [1, 7, 3]),
//             "ActivityRule.SelectedDay.OutOfRange"
//         ];
//     }
//     
//     public static IEnumerable<object[]> GetValidModesForSelectedDays()
//     {
//         yield return [Mode.CustomMode];
//     }
//     
//     public static IEnumerable<object[]> GetInvalidModesForSelectedDays()
//     {
//         yield return [Mode.EveryDayMode];
//         yield return [Mode.FirstDayOfWeekMode];
//         yield return [Mode.LastDayOfWeekMode];
//         yield return [Mode.FirstDayOfMonth];
//         yield return [Mode.LastDayOfMonthMode];
//     }
//     
//         public static IEnumerable<object[]> GetEditUnchangedParameters()
//     {
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
//             new EditActivityRuleParams("test", Mode.EveryDayMode, null)
//         ];
//         
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.CustomMode, [1,2,3]),
//             new EditActivityRuleParams("test", Mode.CustomMode, [1,2,3])
//         ];
//     }
//
//     public static IEnumerable<object[]> GetEditChangedParameters()
//     {
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
//             new EditActivityRuleParams("test1", Mode.EveryDayMode, null)
//         ];
//         
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test1", Mode.EveryDayMode, null),
//             new EditActivityRuleParams("test", Mode.EveryDayMode, null)
//         ];
//         
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.FirstDayOfMonth, null),
//             new EditActivityRuleParams("test", Mode.EveryDayMode, null)
//         ];
//         
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
//             new EditActivityRuleParams("test", Mode.FirstDayOfMonth, null)
//         ];
//         
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.CustomMode, [1,2]),
//             new EditActivityRuleParams("test", Mode.EveryDayMode, null)
//         ];
//         
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.CustomMode, [1,2]),
//             new EditActivityRuleParams("test", Mode.CustomMode, [2,3])
//         ];
//         
//         yield return
//         [
//             ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test", Mode.EveryDayMode, null),
//             new EditActivityRuleParams("test", Mode.CustomMode, [1,2])
//         ];
//     }
//     
//     public sealed record EditActivityRuleParams(string Title, string Mode,
//         List<int>? SelectedDays = null);
// }