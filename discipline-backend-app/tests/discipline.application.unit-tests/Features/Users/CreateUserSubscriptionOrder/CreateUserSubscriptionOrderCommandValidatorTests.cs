using discipline.application.Domain.Users.Enums;
using discipline.application.Features.Users;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.CreateUserSubscriptionOrder;

public sealed class CreateUserSubscriptionOrderCommandValidatorTests
{
    private TestValidationResult<CreateUserSubscriptionOrderCommand> Act(CreateUserSubscriptionOrderCommand command)
        => _validator.TestValidate(command);

    [Fact]
    public void Validate_GivenValidArguments_ShouldNotHaveAnyValidationErrors()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), SubscriptionOrderFrequency.Monthly, new string('1', 14),
            "123");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_GivenEmptyUserId_ShouldHaveValidationErrorForUserId()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.NewGuid(),
            Guid.NewGuid(), null, new string('1', 14), "132");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_GivenEmptyId_ShouldHaveValidationErrorForId()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.NewGuid(), Guid.Empty,
            Guid.NewGuid(), null, new string('1', 14), "132");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public void Validate_GivenEmptySubscriptionId_ShouldHaveValidationErrorForSubscriptionId()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.NewGuid(), Guid.NewGuid(),
            Guid.Empty, null, new string('1', 14), "132");
        
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(x => x.SubscriptionId);
    }
    
    #region arrange
    private IValidator<CreateUserSubscriptionOrderCommand> _validator;

    public CreateUserSubscriptionOrderCommandValidatorTests()
    {
        _validator = new CreateUserSubscriptionOrderCommandValidator();
    }
    #endregion
}