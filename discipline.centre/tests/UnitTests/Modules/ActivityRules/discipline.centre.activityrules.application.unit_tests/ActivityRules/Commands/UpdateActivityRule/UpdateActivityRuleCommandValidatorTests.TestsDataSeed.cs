using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandValidatorTests
{
    public static IEnumerable<object[]> GetValidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",
                null),"test_mode", null)
        ];
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),new ActivityRuleDetailsSpecification("test_title",
                "test_note"),"test_mode", [1,2])
        ];
    }

    public static IEnumerable<object[]> GetInvalidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
            new ActivityRuleDetailsSpecification(string.Empty, "test_note"),"test_mode", null),
            $"{nameof(UpdateActivityRuleCommand.Details)}.{nameof(ActivityRuleDetailsSpecification.Title)}"
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification(
                    new string('t', 31), "test_note"), string.Empty, null),
            nameof(UpdateActivityRuleCommand.Mode)
        ];
    }
}