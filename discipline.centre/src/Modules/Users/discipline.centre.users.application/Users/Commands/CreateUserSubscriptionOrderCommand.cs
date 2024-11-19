using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using FluentValidation;

namespace discipline.centre.users.application.Users.Commands;

public sealed record CreateUserSubscriptionOrderCommand(UserId UserId, SubscriptionOrderId Id, 
    SubscriptionId SubscriptionId, SubscriptionOrderFrequency? SubscriptionOrderFrequency,
    string PaymentToken) : ICommand;
    
public sealed class CreateUserSubscriptionOrderCommandValidator : AbstractValidator<CreateUserSubscriptionOrderCommand>
{
    public CreateUserSubscriptionOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(userId => userId != UserId.Empty())
            .WithMessage("\"User Id\" can not be empty");

        RuleFor(x => x.Id)
            .Must(id => id != new SubscriptionOrderId(Ulid.Empty))
            .WithMessage("\"Id\" can not be empty");
        
        RuleFor(x => x.SubscriptionId)
            .Must(subscriptionId => subscriptionId != new SubscriptionId(Ulid.Empty))
            .WithMessage("\"SubscriptionId\" can not be empty");
    }
}