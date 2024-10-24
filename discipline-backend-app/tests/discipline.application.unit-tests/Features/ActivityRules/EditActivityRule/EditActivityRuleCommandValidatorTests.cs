using discipline.application.Features.ActivityRules;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.ActivityRules.EditActivityRule;

public class EditActivityRuleCommandValidatorTests
{
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new EditActivityRuleCommand(new ActivityRuleId(Ulid.Empty), "Title", "Mode", null);
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("T")]
    [InlineData("TestsTestsTestsTestsTestsTestsTestsTestsTests")]
    public void Validate_GivenInvalid_ShouldHaveValidationErrorForTitle(string title)
    {
        //arrange
        var command = new EditActivityRuleCommand(ActivityRuleId.New(), title, "Mode", null);
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_GivenInvalidMode_ShouldHaveValidationErrorForMode(string mode)
    {
        //arrange
        var command = new EditActivityRuleCommand(ActivityRuleId.New(), "Title", mode, null);
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Mode);
    }
    
    #region arrange
    private IValidator<EditActivityRuleCommand> _validator;
    
    public EditActivityRuleCommandValidatorTests()
    {
        _validator = new EditActivityRuleCommandValidator();
    }
    #endregion
}