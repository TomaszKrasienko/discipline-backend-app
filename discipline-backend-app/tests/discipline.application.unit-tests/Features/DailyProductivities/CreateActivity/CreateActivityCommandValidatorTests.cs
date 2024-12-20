using discipline.application.Features.DailyProductivities;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.CreateActivity;

public sealed class CreateActivityCommandValidatorTests
{
    private TestValidationResult<CreateActivityCommand> Act(CreateActivityCommand command)
        => _validator.TestValidate(command);
    
    [Fact]
    public void Validate_GivenValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new CreateActivityCommand(ActivityId.New(), UserId.New(), "Tests", DateOnly.FromDateTime(DateTime.Now));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new CreateActivityCommand(new ActivityId(Ulid.Empty), UserId.New(), "Title", DateOnly.FromDateTime(DateTime.Now));
    
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorForUserId()
    {
        //arrange
        var command = new CreateActivityCommand(ActivityId.New(), new UserId(Ulid.Empty), "test_title", new DateOnly(2024, 1, 1));
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_GivenEmptyOrNullTitle_ShouldHaveValidationErrorForTitle(string title)
    {
        //arrange
        var command = new CreateActivityCommand(ActivityId.New(), UserId.New(), title, DateOnly.FromDateTime(DateTime.Now));
    
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
    
    [Theory]
    [InlineData('t', 2)]
    [InlineData('t', 101)]
    public void Validate_GivenInvalidTitle_ShouldHaveValidationErrorForId(char character, int multiplier)
    {
        //arrange
        var title = new string(character, multiplier);
        var command = new CreateActivityCommand(ActivityId.New(), UserId.New(), title, DateOnly.FromDateTime(DateTime.Now));
    
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_GivenEmptyDateTime_ShouldHaveValidationErrorForDay()
    {
        //arrange
        var command = new CreateActivityCommand(ActivityId.New(), UserId.New(), "Title", DateOnly.FromDateTime(default));
    
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Day);
    }
    
    #region arrange
    private readonly IValidator<CreateActivityCommand> _validator;

    public CreateActivityCommandValidatorTests()
        => _validator = new CreateActivityCommandValidator();
    #endregion
}