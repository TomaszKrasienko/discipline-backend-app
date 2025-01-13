using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.domain.Users.Services;
using FluentValidation;

namespace discipline.centre.users.application.Users.Commands;

public sealed record CreateUserSubscriptionOrderCommand(UserId UserId, SubscriptionOrderId Id, 
    SubscriptionId SubscriptionId, SubscriptionOrderFrequency? SubscriptionOrderFrequency,
    string? PaymentToken) : ICommand;
    
public sealed class CreateUserSubscriptionOrderCommandValidator : AbstractValidator<CreateUserSubscriptionOrderCommand>
{
    public CreateUserSubscriptionOrderCommandValidator()
    {
        RuleFor(x => x.SubscriptionId)
            .Must(subscriptionId => subscriptionId != new SubscriptionId(Ulid.Empty))
            .WithMessage("\"SubscriptionId\" can not be empty");
    }
}

internal sealed class CreateUserSubscriptionOrderCommandHandler(
    IReadUserRepository readUserRepository,
    IWriteUserRepository writeUserRepository,
    IReadSubscriptionRepository readSubscriptionRepository,
    ISubscriptionOrderService subscriptionOrderService,
    IClock clock) : ICommandHandler<CreateUserSubscriptionOrderCommand>
{
    public async Task HandleAsync(CreateUserSubscriptionOrderCommand command, CancellationToken cancellationToken = default)
    {
        var user = await readUserRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("CreateUserSubscriptionOrder.User", nameof(User), command.UserId.ToString());
        }

        var subscription = await readSubscriptionRepository.GetByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
        {
            throw new NotFoundException("CreateUserSubscriptionOrder.Subscription", nameof(Subscription), command.SubscriptionId.ToString());
        }
        
        subscriptionOrderService.AddOrderSubscriptionToUser(user, command.Id, subscription,
            command.SubscriptionOrderFrequency, clock.DateTimeNow(), command.PaymentToken);
        await writeUserRepository.UpdateAsync(user, cancellationToken);
    }
}