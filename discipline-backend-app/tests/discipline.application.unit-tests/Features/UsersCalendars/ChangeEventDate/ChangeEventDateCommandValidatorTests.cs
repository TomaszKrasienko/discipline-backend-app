using discipline.application.Features.UsersCalendars;
using discipline.domain.SharedKernel.TypeIdentifiers;
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
        var command = new ChangeEventDateCommand(UserId.New(), EventId.New(), new DateOnly(2024,1,1));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorForUserId()
    {
        //arrange
        var command = new ChangeEventDateCommand(new UserId(Ulid.Empty), EventId.New(), new DateOnly(2024, 1, 1));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
    
    [Fact]
    public void Validate_GivenEmptyEventId_ShouldHaveValidationErrorForEventId()
    {
        //arrange
        var command = new ChangeEventDateCommand(UserId.New(), new EventId(Ulid.Empty), new DateOnly(2024, 1, 1));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.EventId);
    }

    [Fact]
    public void Validate_GivenMinDateOnlyAsNewDate_ShouldHaveValidationErrorForNewDate()
    {
        //arrange
        var command = new ChangeEventDateCommand(UserId.New(), EventId.New(), DateOnly.MinValue);
        
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