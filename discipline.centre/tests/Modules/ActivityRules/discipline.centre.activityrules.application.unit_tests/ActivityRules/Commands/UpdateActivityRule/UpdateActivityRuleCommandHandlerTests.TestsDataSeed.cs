using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.tests.sharedkernel.Domain;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandHandlerTests
{
    public static IEnumerable<object[]> GetNotChangedUpdateActivityRuleData()
    {
        var activityRule1 = ActivityRuleFakeDateFactory.Get();
        var command1 = new UpdateActivityRuleCommand(activityRule1.Id, activityRule1.Title,
            activityRule1.Mode, null);

        var selectedDays = new List<int> { 1, 2, 3 };
        var activityRule2 = ActivityRuleFakeDateFactory.Get(selectedDays);
        var command2 = new UpdateActivityRuleCommand(activityRule1.Id, activityRule1.Title,
            activityRule1.Mode, selectedDays);
        
        yield return
        [
            activityRule1, command1
        ];

        yield return
        [
            activityRule2, command2
        ];
    }
}