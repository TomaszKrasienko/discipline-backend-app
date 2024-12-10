using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandHandlerTests
{
    public static IEnumerable<object[]> GetValidUpdateActivityRuleCommand()
    {
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                new ActivityRuleDetailsSpecification("test_title", null), Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                new ActivityRuleDetailsSpecification("test_title", "test_note"), Mode.CustomMode, [1, 2, 3])
        ];
    }
    
    public static IEnumerable<object[]> GetNotChangedUpdateActivityRuleData()
    {
        var activityRule1 = ActivityRuleFakeDataFactory.Get();
        var command1 = new UpdateActivityRuleCommand(activityRule1.Id, activityRule1.UserId, 
            new ActivityRuleDetailsSpecification(activityRule1.Details.Title, activityRule1.Details.Note),
            activityRule1.Mode, null);

        var selectedDays = new List<int> { 1, 2, 3 };
        var activityRule2 = ActivityRuleFakeDataFactory.Get(false, selectedDays);
        var command2 = new UpdateActivityRuleCommand(activityRule2.Id, activityRule2.UserId, 
            new ActivityRuleDetailsSpecification(activityRule2.Details.Title, activityRule2.Details.Note),
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
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification(string.Empty, null),Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                new ActivityRuleDetailsSpecification(new string('t', 31), null), 
                Mode.EveryDayMode, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                new ActivityRuleDetailsSpecification("test_title", null),
                string.Empty, null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", null), "test", null)
        ];
        
        yield return
        [
            new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(),
                new ActivityRuleDetailsSpecification("test_title", null),
                Mode.CustomMode, null)
        ];
    }
}