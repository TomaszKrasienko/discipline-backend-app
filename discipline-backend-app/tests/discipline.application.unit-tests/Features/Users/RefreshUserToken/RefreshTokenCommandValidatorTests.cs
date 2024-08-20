using discipline.application.Features.Users;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.RefreshUserToken;

public sealed class RefreshTokenCommandValidatorTests
{
    private TestValidationResult<RefreshTokenCommand> Act(RefreshTokenCommand command) =>
        _validator.TestValidate(command);
    
    [Fact]
    public void Validate_GivenValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //act
        var command = new RefreshTokenCommand(Guid.NewGuid().ToString());
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_GivenEmptyRefreshToken_ShouldHaveValidationErrorFor()
    {
        //arrange
        var command = new RefreshTokenCommand(string.Empty);
        
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
    }
    
    #region arrange
    private readonly IValidator<RefreshTokenCommand> _validator;

    public RefreshTokenCommandValidatorTests()
    {
        _validator = new RefreshTokenCommandValidator();
    }
    #endregion
}