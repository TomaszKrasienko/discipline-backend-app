using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public partial class CreateActivityRuleCommandValidatorTests
{
    public static IEnumerable<object[]> GetValidCreateActivityRuleCommand()
    {
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Title", "Mode", null)
        ];
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                "Title", "Mode", [1,2])
        ];
    }
    
    public static IEnumerable<object[]> GetInvalidCreateActivityRuleCommand()
    {
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(),
                UserId.New(), string.Empty, "test_mode", null),
            nameof(CreateActivityRuleCommand.Title)
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(),
                UserId.New(), "test_title", string.Empty, null),
            nameof(CreateActivityRuleCommand.Mode)
        ];
    }
}