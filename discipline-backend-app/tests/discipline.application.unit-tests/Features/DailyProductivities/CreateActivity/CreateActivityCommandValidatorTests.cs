using discipline.application.Features.DailyProductivities;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.CreateActivity;

public sealed class CreateActivityCommandValidatorTests
{
    [Fact]
    public void Validate_GivenValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new CreateActivityCommand(Guid.NewGuid(), "Tests", DateOnly.FromDateTime(DateTime.Now));
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new CreateActivityCommand(Guid.Empty, "Title", DateOnly.FromDateTime(DateTime.Now));
    
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Te")]
    [InlineData("12345678902234567890323456789042345678905234567890623456789072345678908234567890923456789002345678901")]
    public void Validate_GivenInvalidTitle_ShouldHaveValidationErrorForId(string title)
    {
        //arrange
        var command = new CreateActivityCommand(Guid.NewGuid(), title, DateOnly.FromDateTime(DateTime.Now));
    
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_GivenEmptyDateTime_ShouldHaveValidationErrorForDay()
    {
        //arrange
        var command = new CreateActivityCommand(Guid.NewGuid(), "Title", DateOnly.FromDateTime(default));
    
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