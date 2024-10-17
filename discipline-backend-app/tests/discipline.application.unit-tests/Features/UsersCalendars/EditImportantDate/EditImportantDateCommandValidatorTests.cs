using discipline.application.Features.UsersCalendars;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.EditImportantDate;

public sealed class EditImportantDateCommandValidatorTests
{
    private TestValidationResult<EditImportantDateCommand> Act(EditImportantDateCommand command)
        => _validator.TestValidate(command);
    
    [Fact]
    public void Validate_GivenValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new EditImportantDateCommand( Guid.NewGuid(), Guid.NewGuid(),
            "test_title");
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorForUserId()
    {
        //arrange
        var command = new EditImportantDateCommand( Guid.Empty, Guid.NewGuid(),
            "test_title");
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
    
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new EditImportantDateCommand( Guid.NewGuid(), Guid.Empty,
            "test_title");
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public void Validate_GivenNullTitle_ShouldHaveValidationErrorForTitle()
    {
        //arrange
        var command = new EditImportantDateCommand( Guid.NewGuid(), Guid.NewGuid(),
            null);
        
        //act
        var result = Act(command);
        
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
        var command = new EditImportantDateCommand( Guid.NewGuid(),Guid.NewGuid(),
            title); 
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
    
    #region arrange
    private readonly IValidator<EditImportantDateCommand> _validator;

    public EditImportantDateCommandValidatorTests()
        => _validator = new EditImportantDateCommandValidator();
    #endregion
}