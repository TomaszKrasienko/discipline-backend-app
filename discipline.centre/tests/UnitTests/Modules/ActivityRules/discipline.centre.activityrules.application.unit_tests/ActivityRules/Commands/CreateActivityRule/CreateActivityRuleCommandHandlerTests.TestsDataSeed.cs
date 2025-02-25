using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.Specifications;
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
            new CreateActivityRuleCommand(UserId.New(), ActivityRuleId.New(), new ActivityRuleDetailsSpecification(
                string.Empty, null), Mode.EveryDayMode, null,null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(UserId.New(), ActivityRuleId.New(), new ActivityRuleDetailsSpecification(
                "Rule title", null), string.Empty, null,null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(UserId.New(), ActivityRuleId.New(), new ActivityRuleDetailsSpecification(
                "Rule title", null), "test_mode", null,null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(UserId.New(), ActivityRuleId.New(), new ActivityRuleDetailsSpecification(
                "Rule title", null), Mode.CustomMode, null,null)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(UserId.New(), ActivityRuleId.New(), new ActivityRuleDetailsSpecification(
                "Rule title", null), Mode.EveryDayMode, [1,2],null)
        ];
    }
}