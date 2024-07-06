using discipline.application.Features.ActivityRules;
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
        var command = new EditActivityRuleCommand(Guid.Empty, "Title", "Mode", null);
        
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
        var command = new EditActivityRuleCommand(Guid.NewGuid(), title, "Mode", null);
        
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
        var command = new EditActivityRuleCommand(Guid.NewGuid(), "Title", mode, null);
        
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