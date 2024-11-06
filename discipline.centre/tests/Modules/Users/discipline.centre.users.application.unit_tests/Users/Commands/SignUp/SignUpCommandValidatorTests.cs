using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.application.unit_tests.Users.Commands.SignUp;

public sealed class SignUpCommandValidatorTests
{
    private TestValidationResult<SignUpCommand> Act(SignUpCommand command) => _validator.TestValidate(command);

    [Theory]
    [MemberData(nameof(GetValidSignUpCommand))]
    public void Validate_GivenValidSignUpCommand_ShouldNotHaveAnyValidationErrors(SignUpCommand command)
    {
        //assert
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    public static IEnumerable<object[]> GetValidSignUpCommand()
    {
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name")];
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidSignUpCommand))]
    public void Validate_GivenInvalidSignUpCommand_ShouldHaveValidationErrorsFor(SignUpCommand command,
        string fieldName)
    {
        //assert
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
    }
    
    public static IEnumerable<object[]> GetInvalidSignUpCommand()
    {
        yield return [new SignUpCommand(new UserId(Ulid.Empty), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name"), nameof(SignUpCommand.Id)];
        
        yield return [new SignUpCommand(UserId.New(), string.Empty, "Test123!",
            "test_first_name", "test_last_name"), nameof(SignUpCommand.Email)];
        
        yield return [new SignUpCommand(UserId.New(), "test_invalid_email", "Test123!",
            "test_first_name", "test_last_name"), nameof(SignUpCommand.Email)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "TEST123!",
            "test_first_name", "test_last_name"), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "test123!",
            "test_first_name", "test_last_name"), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test!",
            "test_first_name", "test_last_name"), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123",
            "test_first_name", "test_last_name"), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            string.Empty, "test_last_name"), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            new string('t', 1), "test_last_name"), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            new string('t', 101), "test_last_name"), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "test_first_name", string.Empty), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "test_first_name", new string('t',1)), nameof(SignUpCommand.Password)];
        
        yield return [new SignUpCommand(UserId.New(), "test@test.pl", "Test123!",
            "test_first_name", new string('t',101)), nameof(SignUpCommand.Password)];
    }

    #region arrange
    private readonly IValidator<SignUpCommand> _validator;
    
    public SignUpCommandValidatorTests()
    {
        _validator = new SignUpCommandValidator();
    }
    #endregion
}