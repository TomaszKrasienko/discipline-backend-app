using discipline.application.Features.Users;
using FluentValidation;
using FluentValidation.TestHelper;

namespace discipline.application.unit_tests.Features.Users.CreateUserSubscriptionOrder;

public sealed class CreateUserSubscriptionOrderCommandValidatorTests
{
    private TestValidationResult<CreateUserSubscriptionOrderCommand> Act(CreateUserSubscriptionOrderCommand command)
        => _validator.TestValidate(command);
    
    #region arrange
    private IValidator<CreateUserSubscriptionOrderCommand> _validator;

    public CreateUserSubscriptionOrderCommandValidatorTests()
    {
        _validator = new CreateUserSubscriptionOrderCommandValidator();
    }
    #endregion
}