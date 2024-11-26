using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public sealed class CreateActivityRuleCommandValidatorTests
{
    private TestValidationResult<CreateActivityRuleCommand> Act(CreateActivityRuleCommand command) 
        => _validator.TestValidate(command);

    [Theory]
    [MemberData(nameof(GetValidCreateActivityRuleCommand))]
    public void Validate_GivenValidCommand_ShouldNotHaveAnyValidationErrors(CreateActivityRuleCommand command)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

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

    [Theory]
    [MemberData(nameof(GetInvalidCreateActivityRuleCommand))]
    public void Validate_GivenInvalidCommand_ShouldHaveValidationErrorFor(CreateActivityRuleCommand command, string fieldName)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
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
    
    #region arrange
    private readonly IValidator<CreateActivityRuleCommand> _validator;

    public CreateActivityRuleCommandValidatorTests()
    {
        _validator = new CreateActivityRuleCommandValidator();
    }
    #endregion
}