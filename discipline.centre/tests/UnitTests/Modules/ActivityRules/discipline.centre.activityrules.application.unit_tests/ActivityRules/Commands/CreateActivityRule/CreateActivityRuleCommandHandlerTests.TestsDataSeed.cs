using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public partial class CreateActivityRuleCommandHandlerTests
{
    public static IEnumerable<object[]> GetInvalidCreateActivityRuleCommand()
    {
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                string.Empty, Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", string.Empty, null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", "test_mode", null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", Mode.CustomMode, null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", Mode.EveryDayMode, [1,2])
        ];
    }
}