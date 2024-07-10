using discipline.application.Features.UsersCalendars;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.AddImportantDate;

public sealed class AddImportantDateCommandValidatorTests
{
    [Fact]
    public void Validate_GivenValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new AddImportantDateCommand(new DateOnly(2024, 1, 1), Guid.NewGuid(),
            "test_title");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new AddImportantDateCommand(new DateOnly(2024, 1, 1), Guid.Empty,
            "test_title");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public void Validate_GivenNullTitle_ShouldHaveValidationErrorForTitle()
    {
        //arrange
        var command = new AddImportantDateCommand(new DateOnly(2024, 1, 1), Guid.NewGuid(),
            null);
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData('t', 1)]
    [InlineData('t', 101)]
    public void Validate_GivenInvalidTitle_ShouldHaveValidationErrorForTitle(char letter, int multiplier)
    {
        //arrange
        var title = new string(letter, multiplier);
        var command = new AddImportantDateCommand(new DateOnly(2024, 1, 1), Guid.NewGuid(),
            title);
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    #region arrange
    private readonly IValidator<AddImportantDateCommand> _validator;

    public AddImportantDateCommandValidatorTests()
        => _validator = new AddImportantDateCommandValidator();
    #endregion
}