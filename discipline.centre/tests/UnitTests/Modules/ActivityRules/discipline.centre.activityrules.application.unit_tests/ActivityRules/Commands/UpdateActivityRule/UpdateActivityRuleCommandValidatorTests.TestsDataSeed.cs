using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandValidatorTests
{
    public static IEnumerable<object[]> GetValidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), "test_title", null,
                "test_mode", null)
        ];
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),"test_title", "test_note","test_mode", [1,2])
        ];
    }

    public static IEnumerable<object[]> GetInvalidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),string.Empty,
                "test_mode", null),
            nameof(UpdateActivityRuleCommand.Title)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), new string('t', 31),
                string.Empty, null),
            nameof(UpdateActivityRuleCommand.Mode)
        ];
    }
}