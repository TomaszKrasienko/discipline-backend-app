using discipline.application.Features.UsersCalendars;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.EditCalendarEvent;

public sealed class EditCalendarEventValidatorTests
{
    [Fact]
    public void Validate_GivenValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new EditCalendarEventCommand( Guid.NewGuid(), Guid.NewGuid(),
            "test_title", new TimeOnly(11, 00),  null, "test_action");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorForUserId()
    {
        //arrange
        var command = new EditCalendarEventCommand( Guid.Empty, Guid.NewGuid(),
            "test_title", new TimeOnly(11, 00),  null, "test_action");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
    
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new EditCalendarEventCommand( Guid.NewGuid(), Guid.Empty,
            "test_title", new TimeOnly(11, 00),  null, "test_action");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public void Validate_GivenNullTitle_ShouldHaveValidationErrorForTitle()
    {
        //arrange
        var command = new EditCalendarEventCommand( Guid.NewGuid(), Guid.NewGuid(),
            null, new TimeOnly(11, 00),  null, "test_action");
        
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
        var command = new EditCalendarEventCommand( Guid.NewGuid(),Guid.NewGuid(),
            title, new TimeOnly(11, 00),  null, "test_action"); 
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_GivenEmptyTimeFrom_ShouldHaveValidationErrorForTimeFrom()
    {
        //arrange
        var command = new EditCalendarEventCommand( Guid.NewGuid(), Guid.NewGuid(),
            "test_title", default, null, "test_action");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.TimeFrom);
    }
    
    #region arrange
    private readonly IValidator<EditCalendarEventCommand> _validator;

    public EditCalendarEventValidatorTests()
        => _validator = new EditCalendarEventCommandValidator();
    #endregion
}