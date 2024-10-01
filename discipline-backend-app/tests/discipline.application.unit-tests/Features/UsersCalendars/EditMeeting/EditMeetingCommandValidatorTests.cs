using discipline.application.Features.UsersCalendars;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.EditMeeting;

public sealed class EditMeetingCommandValidatorTests
{
    private TestValidationResult<EditMeetingCommand> Act(EditMeetingCommand command)
        => _validator.TestValidate(command);
    
    [Fact]
    public void Validate_GivenValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new EditMeetingCommand(Guid.NewGuid(), Guid.NewGuid(),
            "test_title", new TimeOnly(11, 00),  new TimeOnly(12, 00), 
            "platform", "uri", "place");
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new EditMeetingCommand(Guid.NewGuid(), Guid.Empty,
            "test_title", new TimeOnly(11, 00),  new TimeOnly(12, 00), 
            "platform", "uri", "place");
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorForUserId()
    {
        //arrange
        var command = new EditMeetingCommand( Guid.Empty, Guid.NewGuid(),
            "test_title", new TimeOnly(11, 00),  new TimeOnly(12, 00), 
            "platform", "uri", "place");
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
    
    [Fact]
    public void Validate_GivenNullTitle_ShouldHaveValidationErrorForTitle()
    {
        //arrange
        var command = new EditMeetingCommand(Guid.NewGuid(), Guid.NewGuid(),
            null, new TimeOnly(11, 00),new TimeOnly(12, 00), 
            "platform", "uri", "place");
        
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
        var command = new EditMeetingCommand(Guid.NewGuid(),Guid.NewGuid(),
            title, new TimeOnly(11, 00),  new TimeOnly(12, 00), 
            "platform", "uri", "place"); 
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
    
    [Fact]
    public void Validate_GivenEmptyTimeFrom_ShouldHaveValidationErrorForTimeFrom()
    {
        //arrange
        var command = new EditMeetingCommand( Guid.NewGuid(),Guid.NewGuid(),
            "test_title", default, new TimeOnly(12, 00), 
            "platform", "uri", "place");
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.TimeFrom);
    }
    
    #region arrange
    private readonly IValidator<EditMeetingCommand> _validator;

    public EditMeetingCommandValidatorTests()
        => _validator = new EditMeetingCommandValidator();
    #endregion
}