using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public partial class CreateActivityRuleCommandValidatorTests
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

    [Theory]
    [MemberData(nameof(GetInvalidCreateActivityRuleCommand))]
    public void Validate_GivenInvalidCommand_ShouldHaveValidationErrorFor(CreateActivityRuleCommand command, string fieldName)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
    }
    
    #region arrange
    private readonly IValidator<CreateActivityRuleCommand> _validator;

    public CreateActivityRuleCommandValidatorTests()
    {
        _validator = new CreateActivityRuleCommandValidator();
    }
    #endregion
}