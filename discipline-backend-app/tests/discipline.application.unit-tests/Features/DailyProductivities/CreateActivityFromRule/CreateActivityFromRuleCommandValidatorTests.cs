using discipline.application.Features.DailyProductivities;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.CreateActivityFromRule;

public sealed class CreateActivityFromRuleCommandValidatorTests
{
    [Fact]
    public void Validate_GivenValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new CreateActivityFromRuleCommand(ActivityId.New(), ActivityRuleId.New());
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();   
    }
    
    [Fact]
    public void Validate_GivenEmptyActivityRuleId_ShouldHaveValidationErrorForActivityRuleId()
    {
        //arrange
        var command = new CreateActivityFromRuleCommand(ActivityId.New(), new ActivityRuleId(Ulid.Empty));
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.ActivityRuleId);
    }
    
    [Fact]
    public void Validate_GivenEmptyActivityId_ShouldHaveValidationErrorForActivityId()
    {
        //arrange
        var command = new CreateActivityFromRuleCommand(new ActivityId(Ulid.Empty), ActivityRuleId.New());
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.ActivityId);
    }
    
    #region arrange
    private readonly IValidator<CreateActivityFromRuleCommand> _validator;

    public CreateActivityFromRuleCommandValidatorTests()
        => _validator = new CreateActivityFromRuleCommandValidator();
    #endregion
}