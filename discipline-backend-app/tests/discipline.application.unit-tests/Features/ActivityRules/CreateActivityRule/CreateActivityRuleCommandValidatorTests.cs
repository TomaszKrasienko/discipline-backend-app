using discipline.application.Features.ActivityRules;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;
namespace discipline.application.unit_tests.Features.ActivityRules.CreateActivityRule;

public sealed class CreateActivityRuleCommandValidatorTests
{
    private TestValidationResult<CreateActivityRuleCommand> Act(CreateActivityRuleCommand command)
        => _validator.TestValidate(command);

    [Fact]
    public void Validate_GivenValidCommand_ShouldNotHaveAnyValidationError()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Title", "Mode", null);
        
        //act
        var result = Act(command);

        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Title", "Mode", null);
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorFor()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Title", "Mode", null);
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("T")]
    [InlineData("12345678902234567890323456789042345678905234567890623456789072345678908234567890923456789002345678901")]
    public void Validate_GivenInvalidTitle_ShouldHaveValidationErrorForTitle(string title)
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), 
            UserId.New(), title, "Mode", null);
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_GivenInvalidMode_ShouldHaveValidationErrorForMode(string mode)
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Title", mode, null);
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Mode);
    }

    #region arrange
    private readonly IValidator<CreateActivityRuleCommand> _validator;

    public CreateActivityRuleCommandValidatorTests()
        => _validator = new CreateActivityRuleCommandValidator();
    #endregion
}