using discipline.application.Features.ActivityRules;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;
namespace discipline.application.unit_tests.Features.ActivityRules.CreateActivityRule;

public sealed class CreateActivityRuleCommandValidatorTests
{
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new CreateActivityRuleCommand(Guid.Empty, "Title", "Mode", null);
        
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
        var command = new CreateActivityRuleCommand(Guid.NewGuid(), title, "Mode", null);
        
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
        var command = new CreateActivityRuleCommand(Guid.NewGuid(), "Title", mode, null);
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Mode);
    }

    #region arrange
    private readonly IValidator<CreateActivityRuleCommand> _validator;

    public CreateActivityRuleCommandValidatorTests()
    {
        _validator = new CreateActivityRuleCommandValidator();
    }
    #endregion
}