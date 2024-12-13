using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.domain.Users.Enums;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.users.application.unit_tests.Users.Commands.CreateSubscriptionOrder;

public sealed class CreateUserSubscriptionOrderCommandValidatorTests
{
    private TestValidationResult<CreateUserSubscriptionOrderCommand> Act(CreateUserSubscriptionOrderCommand command)
        => _validator.TestValidate(command);

    [Theory]
    [MemberData(nameof(GetValidCreateSubscriptionOrder))]
    public void Validate_GivenValidCreateSubscriptionCommand_ShouldNotHaveAnyValidationErrors(
        CreateUserSubscriptionOrderCommand command)
    {
        //act
        var result = _validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidCreateSubscriptionOrder()
    {
        yield return
        [
            new CreateUserSubscriptionOrderCommand(UserId.New(), SubscriptionOrderId.New(),
                SubscriptionId.New(), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString())
        ];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateSubscriptionCommand))]
    public void Validate_GivenInvalidCreateSubscriptionCommand_ShouldHaveValidationErrorsFor(
        CreateUserSubscriptionOrderCommand command, string fieldName)
    {
        //assert
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
    }

    public static IEnumerable<object[]> GetInvalidCreateSubscriptionCommand()
    {
        yield return
        [
            new CreateUserSubscriptionOrderCommand(new UserId(Ulid.Empty), SubscriptionOrderId.New(), 
            SubscriptionId.New(), null, Guid.NewGuid().ToString()),
            nameof(CreateUserSubscriptionOrderCommand.UserId)
        ];
        
        yield return
        [
            new CreateUserSubscriptionOrderCommand(UserId.New(), new SubscriptionOrderId(Ulid.Empty),
                SubscriptionId.New(), null, Guid.NewGuid().ToString()),
            nameof(CreateUserSubscriptionOrderCommand.Id)
        ];
        
        yield return
        [
            new CreateUserSubscriptionOrderCommand(UserId.New(), SubscriptionOrderId.New(), 
                new SubscriptionId(Ulid.Empty), null,  Guid.NewGuid().ToString()),
            nameof(CreateUserSubscriptionOrderCommand.SubscriptionId)
        ];
    }
    
    #region arrange
    private readonly IValidator<CreateUserSubscriptionOrderCommand> _validator;
    
    public CreateUserSubscriptionOrderCommandValidatorTests()
    {
        _validator = new CreateUserSubscriptionOrderCommandValidator();
    }
    #endregion
}