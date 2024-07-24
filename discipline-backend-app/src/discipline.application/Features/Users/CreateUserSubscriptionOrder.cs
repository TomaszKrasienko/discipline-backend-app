using System.Data;
using discipline.application.Behaviours;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Repositories;
using discipline.application.Exceptions;
using FluentValidation;

namespace discipline.application.Features.Users;

public class CreateUserSubscriptionOrder
{
    
}

public sealed record CreateUserSubscriptionOrderCommand(Guid UserId, Guid Id, 
    Guid SubscriptionId, SubscriptionOrderFrequency? SubscriptionOrderFrequency,
    string CardNumber, string CardCvvNumber) : ICommand;

public sealed class CreateUserSubscriptionOrderCommandValidator : AbstractValidator<CreateUserSubscriptionOrderCommand>
{
    public CreateUserSubscriptionOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("\"User Id\" can not be empty");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("\"Id\" can not be empty");
        
        RuleFor(x => x.SubscriptionId)
            .NotEmpty()
            .WithMessage("\"SubscriptionId\" can not be empty");

        RuleFor(x => x.CardNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("\"Card number\" can not be empty");

        RuleFor(x => x.CardNumber)
            .MinimumLength(13)
            .MaximumLength(19)
            .WithMessage("\"Card number\" has invalid length");
        
        RuleFor(x => x.CardCvvNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("\"Card Cvv Number\" can not be empty");

        RuleFor(x => x.CardCvvNumber)
            .Length(3)
            .WithMessage("\"Card Cvv Number\" has invalid length");
    }
}

internal sealed class CreateUserSubscriptionOrderCommandHandler(
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository,
    IClock clock) : ICommandHandler<CreateUserSubscriptionOrderCommand>
{
    public async Task HandleAsync(CreateUserSubscriptionOrderCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }

        var subscription = await subscriptionRepository.GetByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
        {
            throw new SubscriptionNotFoundException(command.SubscriptionId);
        }
        
        user.CreateSubscriptionOrder(command.Id, subscription, command.SubscriptionOrderFrequency, clock.DateNow(), command.CardNumber,
            command.CardCvvNumber);
        await userRepository.UpdateAsync(user, cancellationToken);
    }
}