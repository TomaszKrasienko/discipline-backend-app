using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public sealed class UpdateActivityRuleCommandValidatorTests
{
    private TestValidationResult<UpdateActivityRuleCommand> Act(UpdateActivityRuleCommand command)
        => _validator.TestValidate(command);
    
    [Theory]
    [MemberData(nameof(GetValidUpdateActivityRuleCommand))]
    public void Validate_GivenValidCommand_ShouldNotHaveAnyValidationErrors(UpdateActivityRuleCommand command)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

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

    [Theory]
    [MemberData(nameof(GetInvalidUpdateActivityRuleCommand))]
    public void Validate_GivenInvalidCommand_ShouldHaveValidationErrorFor(UpdateActivityRuleCommand command, string fieldName)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
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
    
    #region arrange
    private readonly IValidator<UpdateActivityRuleCommand> _validator;

    public UpdateActivityRuleCommandValidatorTests()
        => _validator = new UpdateActivityRuleCommandValidator();
    #endregion
}