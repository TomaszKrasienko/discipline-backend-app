using discipline.application.Features.Users;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.SignIn;

public sealed class SignInCommandValidatorTests
{
    [Fact]
    public void Validate_GivenEmptyEmail_ShouldShaveValidationErrorsForEmail()
    {
        //arrange
        var command = new SignInCommand(string.Empty, "Tests123!");
        
        //assert
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Fact]
    public void Validate_GivenInvalidEmail_ShouldShaveValidationErrorsForEmail()
    {
        //arrange
        var command = new SignInCommand("test", "Tests123!");
        
        //assert
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Fact]
    public void Validate_GivenEmptyPassword_ShouldShaveValidationErrorsForPassword()
    {
        //arrange
        var command = new SignInCommand("test@test.pl", string.Empty);
        
        //assert
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    #region arrange
    private readonly IValidator<SignInCommand> _validator;

    public SignInCommandValidatorTests()
        => _validator = new SignInCommandValidator();
    #endregion
}