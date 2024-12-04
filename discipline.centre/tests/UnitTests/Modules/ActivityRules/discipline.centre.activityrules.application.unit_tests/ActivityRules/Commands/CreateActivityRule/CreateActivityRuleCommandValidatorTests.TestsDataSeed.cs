using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public partial class CreateActivityRuleCommandValidatorTests
{
    public static IEnumerable<object[]> GetValidCreateActivityRuleCommand()
    {
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                new ActivityRuleDetailsSpecification("test_title", null), "Mode", null, null)
        ];
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification(
                "test_titel", "test_note"), "Mode", [1,2], null)
        ];
    }
    
    public static IEnumerable<object[]> GetInvalidCreateActivityRuleCommand()
    {
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification(string.Empty, null),
                "test_mode", null, null),
            $"{nameof(CreateActivityRuleCommand.Details)}.{nameof(ActivityRuleDetailsSpecification.Title)}"
        ];
        
        yield return
        [
            new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification(
                    "test_title", null), string.Empty, null, null),
            nameof(CreateActivityRuleCommand.Mode)
        ];
    }
}