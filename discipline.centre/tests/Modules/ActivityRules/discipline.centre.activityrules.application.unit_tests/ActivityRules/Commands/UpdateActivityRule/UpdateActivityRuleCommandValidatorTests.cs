using discipline.centre.activityrules.application.ActivityRules.Commands;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandValidatorTests
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

    [Theory]
    [MemberData(nameof(GetInvalidUpdateActivityRuleCommand))]
    public void Validate_GivenInvalidCommand_ShouldHaveValidationErrorFor(UpdateActivityRuleCommand command, string fieldName)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
    }
    
    #region arrange
    private readonly IValidator<UpdateActivityRuleCommand> _validator;

    public UpdateActivityRuleCommandValidatorTests()
        => _validator = new UpdateActivityRuleCommandValidator();
    #endregion
}