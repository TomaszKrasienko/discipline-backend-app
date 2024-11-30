using discipline.centre.users.application.Users.Commands;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.application.unit_tests.Users.Commands.SignIn;

public sealed class SignInCommandValidatorTests
{
    private TestValidationResult<SignInCommand> Act(SignInCommand command) 
        => _validator.TestValidate(command);
    
    [Theory]
    [MemberData(nameof(GetValidSignInCommand))]
    public void Validate_GivenValidSignInCommand_ShouldNotHaveAnyValidationErrors(SignInCommand command)
    {
        //assert
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidSignInCommand()
    {
        yield return [new SignInCommand("test@test.pl", "Test123!")];
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidSignInCommand))]
    public void Validate_GivenInvalidSignInCommand_ShouldHaveValidationErrorsFor(SignInCommand command,
        string fieldName)
    {
        //assert
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
    }

    public static IEnumerable<object[]> GetInvalidSignInCommand()
    {
        yield return [new SignInCommand(string.Empty, "Tests123!"), nameof(SignInCommand.Email)];
        
        yield return [new SignInCommand("test", "Tests123!"), nameof(SignInCommand.Email)];
        
        yield return [new SignInCommand("test@test.pl", string.Empty), nameof(SignInCommand.Password)];
    }
    
    #region arrange
    private readonly IValidator<SignInCommand> _validator;

    public SignInCommandValidatorTests()
        => _validator = new SignInCommandValidator();
    #endregion
}