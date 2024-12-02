using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class CreateTests
{
    public static IEnumerable<object[]> GetValidCreateActivityRulesData()
    {
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",null,Mode.EveryDayMode)
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title","test_note",
                Mode.CustomMode, [1,2,3])
        ];

        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title", "test_note",
                Mode.EveryDayMode, null, [new StageSpecification("test_stage1", 1),
                    new StageSpecification("test_stage2", 2)])
        ];
    }
    

    public static IEnumerable<object[]> GetInvalidCreateActivityRulesData()
    {
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), string.Empty, null,Mode.EveryDayMode),
                "ActivityRule.Details.Title.Empty"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title", null, string.Empty),
            "ActivityRule.Mode.Empty"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title", null, "test_mode"),
            "ActivityRule.Mode.Unavailable"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title", null, Mode.CustomMode, 
                [-1, 2, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title", null,Mode.CustomMode, 
                [1, 7, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title", null,
                Mode.EveryDayMode, null, [new StageSpecification("test_stage1", 1),
                    new StageSpecification("test_stage2", 3)]),
            "ActivityRule.Stages.MustHaveOrderedIndex"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title", null,
                Mode.EveryDayMode, null, [new StageSpecification("test_stage1", 1),
                    new StageSpecification("test_stage1", 2)]),
            "ActivityRule.Stages.StageTitleMustBeUnique"
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
    
    public sealed record CreateActivityRuleParams(ActivityRuleId Id, UserId UserId, string Title, string? Note, string Mode,
        List<int>? SelectedDays = null, List<StageSpecification>? Stages = null);
}