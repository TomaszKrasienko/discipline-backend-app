using discipline.application.Features.UsersCalendars;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.ChangeEventDate;

public sealed class ChangeEventDateCommandValidatorTests
{
    private TestValidationResult<ChangeEventDateCommand> Act(ChangeEventDateCommand command)
        => _validator.TestValidate(command);

    [Fact]
    public void Validate_GivenAllValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new ChangeEventDateCommand(Guid.NewGuid(), Guid.NewGuid(), new DateOnly(2024,1,1));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorForUserId()
    {
        //arrange
        var command = new ChangeEventDateCommand(Guid.Empty, Guid.NewGuid(), new DateOnly(2024, 1, 1));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.EventId);
    }
    
    [Fact]
    public void Validate_GivenEmptyEventId_ShouldHaveValidationErrorForEventId()
    {
        //arrange
        var command = new ChangeEventDateCommand(Guid.NewGuid(), Guid.Empty, new DateOnly(2024, 1, 1));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.EventId);
    }

    [Fact]
    public void Validate_GivenMinDateOnlyAsNewDate_ShouldHaveValidationErrorForNewDate()
    {
        //arrange
        var command = new ChangeEventDateCommand(Guid.NewGuid(), Guid.NewGuid(), DateOnly.MinValue);
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.NewDate);
    }

    #region arrange
    private readonly IValidator<ChangeEventDateCommand> _validator;

    public ChangeEventDateCommandValidatorTests()
        => _validator = new ChangeEventDateCommandValidator();
    #endregion
}