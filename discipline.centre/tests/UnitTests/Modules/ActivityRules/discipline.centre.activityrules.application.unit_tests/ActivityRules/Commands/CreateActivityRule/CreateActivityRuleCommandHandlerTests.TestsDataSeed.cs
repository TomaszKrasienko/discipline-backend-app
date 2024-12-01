using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public partial class CreateActivityRuleCommandHandlerTests
{
    public static IEnumerable<object[]> GetInvalidCreateActivityRuleCommand()
    {
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                string.Empty, null, Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", null, string.Empty, null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", null, "test_mode", null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", null, Mode.CustomMode, null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Rule title", null, Mode.EveryDayMode, [1,2])
        ];
    }
}