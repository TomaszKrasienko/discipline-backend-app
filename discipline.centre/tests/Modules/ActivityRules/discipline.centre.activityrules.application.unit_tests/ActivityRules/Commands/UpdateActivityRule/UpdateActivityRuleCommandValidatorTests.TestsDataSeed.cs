using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandValidatorTests
{
    public static IEnumerable<object[]> GetValidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "Title", "Mode",
                null)
        ];
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "Title", "Mode", [1,2])
        ];
    }

    public static IEnumerable<object[]> GetInvalidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(),string.Empty,
                "test_mode", null),
            nameof(UpdateActivityRuleCommand.Title)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), "test_title",
                string.Empty, null),
            nameof(UpdateActivityRuleCommand.Mode)
        ];
    }
}