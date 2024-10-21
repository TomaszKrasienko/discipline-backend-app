using discipline.application.Features.DailyProductivities;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.ChangeActivityCheck;

public sealed class ChangeActivityCheckCommandValidatorTests
{
    [Fact]
    public void Validate_GivenValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new ChangeActivityCheckCommand(ActivityId.New());
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyActivityId_ShouldHaveValidationErrorForActivityId()
    {
        //arrange
        var command = new ChangeActivityCheckCommand(new ActivityId(Ulid.Empty));
    
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.ActivityId);
    }
    
    #region arrange
    private readonly IValidator<ChangeActivityCheckCommand> _validator;

    public ChangeActivityCheckCommandValidatorTests()
        => _validator = new ChangeActivityCheckCommandValidator();
    #endregion
}