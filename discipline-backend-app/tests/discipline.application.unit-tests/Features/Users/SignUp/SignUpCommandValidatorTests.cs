using discipline.application.Features.Users;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.SignUp;

public sealed class SignUpCommandValidatorTests
{
    private TestValidationResult<SignUpCommand> Act(SignUpCommand command) => _validator.TestValidate(command);

    [Fact]
    public void Validate_GivenValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name"));
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyGuid_ShouldHaveValidationErrorForId()
    {
        //act
        var result = Act(new SignUpCommand(Guid.Empty, "test@test.pl", "Test123!",
            "test_first_name", "test_last_name"));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_GivenEmptyEmail_ShouldHaveValidationErrorForEmail()
    {
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), string.Empty, "Test123!",
            "test_first_name", "test_last_name"));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Fact]
    public void Validate_GivenInvalidEmail_ShouldHaveValidationErrorForEmail()
    {
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), "test_invalid_email", "Test123!",
            "test_first_name", "test_last_name"));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("TEST123!")]
    [InlineData("test123!")]
    [InlineData("Test!")]
    [InlineData("Test123")]
    public void Validate_GivenInvalidPassword_ShouldHaveValidationErrorForPassword(string password)
    {
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), "test@test.pl", password,
            "test_first_name", "test_last_name"));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_GivenEmptyFirstName_ShouldHaveValidationErrorForFistName()
    {
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), "test@test.pl", "Test123!",
            string.Empty, "test_last_name"));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Theory]
    [InlineData('t', 2)]
    [InlineData('t', 101)]
    public void Validate_GivenInvalidLengthFirstName_ShouldHaveValidationErrorForFistName(char character, int multiplier)
    {
        //arrange
        var firstName = new string(character, multiplier);
        
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), "test@test.pl", "Test123!",
            firstName, "test_last_name"));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Fact]
    public void Validate_GivenEmptyLastName_ShouldHaveValidationErrorForLastName()
    {
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), "test@test.pl", "Test123!",
            "test_first_name", string.Empty));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Theory]
    [InlineData('t', 2)]
    [InlineData('t', 101)]
    public void Validate_GivenInvalidLengthLastName_ShouldHaveValidationErrorForLastName(char character, int multiplier)
    {
        //arrange
        var lastName = new string(character, multiplier);
        
        //act
        var result = Act(new SignUpCommand(Guid.NewGuid(), "test@test.pl", "Test123!",
            "test_first_name", lastName));
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    #region arrange
    private readonly IValidator<SignUpCommand> _validator;
    
    public SignUpCommandValidatorTests()
    {
        _validator = new SignUpCommandValidator();
    }
    #endregion
}