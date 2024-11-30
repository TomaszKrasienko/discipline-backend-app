using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace discipline.centre.application.unit_tests.Subscriptions.Commands.CreateSubscription;

public sealed class CreateSubscriptionCommandValidatorTests
{
    private TestValidationResult<CreateSubscriptionCommand> Act(CreateSubscriptionCommand command)
        => _validator.TestValidate(command);

    [Theory]
    [MemberData(nameof(GetValidCreateSubscriptionCommand))]
    public void Validate_GivenValidCreateSubscriptionCommand_ShouldNotHaveAnyValidationErrors(CreateSubscriptionCommand command)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidCreateSubscriptionCommand()
    {
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), "test", 1m, 1m, ["test"])
        ];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateSubscriptionCommand))]
    public void Validate_GivenInvalidCreateSubscriptionCommand_ShouldHaveValidationErrorFor(CreateSubscriptionCommand command, 
        string fieldName)
    {
        //act
        var result = Act(command);
        
        //assert
        result.ShouldHaveValidationErrorFor(fieldName);
    }

    public static IEnumerable<object[]> GetInvalidCreateSubscriptionCommand()
    {
        yield return 
        [
            new CreateSubscriptionCommand(SubscriptionId.Empty(), "test", 1m, 1m, ["test"]),
            nameof(CreateSubscriptionCommand.Id)
        ];

        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), string.Empty, 1m,1m, ["test"]),
            nameof(CreateSubscriptionCommand.Title)
        ];
        
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", -1m,1m, ["test"]),
            nameof(CreateSubscriptionCommand.PricePerMonth)
        ];
        
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m,-1m, ["test"]),
            nameof(CreateSubscriptionCommand.PricePerYear)
        ];
        
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m,1m, []),
            nameof(CreateSubscriptionCommand.Features)
        ];
    }
    
    #region region
    private readonly IValidator<CreateSubscriptionCommand> _validator;

    public CreateSubscriptionCommandValidatorTests()
    {
        _validator = new CreateSubscriptionCommandValidator();
    }
    #endregion
}