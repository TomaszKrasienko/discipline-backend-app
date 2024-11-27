using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandHandlerTests
{
    public static IEnumerable<object[]> GetValidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "test_title", Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "test_title", Mode.CustomMode, [1, 2, 3])
        ];
    }
    
    public static IEnumerable<object[]> GetNotChangedUpdateActivityRuleData()
    {
        var activityRule1 = ActivityRuleFakeDateFactory.Get();
        var command1 = new UpdateActivityRuleCommand(activityRule1.Id, activityRule1.Title,
            activityRule1.Mode, null);

        var selectedDays = new List<int> { 1, 2, 3 };
        var activityRule2 = ActivityRuleFakeDateFactory.Get(selectedDays);
        var command2 = new UpdateActivityRuleCommand(activityRule2.Id, activityRule2.Title,
            activityRule2.Mode, selectedDays);
        
        yield return
        [
            activityRule1, command1
        ];

        yield return
        [
            activityRule2, command2
        ];
    }
    
    public static IEnumerable<object[]> GetInvalidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), string.Empty, Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), new string('t', 1), Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), new string('t', 101), Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "test_title", string.Empty, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "test_title", "test", null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "test_title", Mode.CustomMode, null)
        ];
    }
}