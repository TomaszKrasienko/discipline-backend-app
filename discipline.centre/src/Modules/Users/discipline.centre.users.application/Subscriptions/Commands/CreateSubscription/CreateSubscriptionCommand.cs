using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using FluentValidation;

namespace discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;

public sealed record class CreateSubscriptionCommand(SubscriptionId Id,
    string Title, decimal PricePerMonth, decimal PricePerYear, List<string> Features) : ICommand;

internal sealed class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => x != SubscriptionId.Empty())
            .WithMessage("Id can not be empty");

        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Title can not be empty");

        RuleFor(x => x.PricePerMonth)
            .Must(x => x > 0)
            .WithMessage("Price per month can not be negative");

        RuleFor(x => x.PricePerYear)
            .Must(x => x > 0)
            .WithMessage("Price per year can not be negative");

        RuleFor(x => x.Features)
            .Must(x => x.Count != 0)
            .WithMessage("Features must have at least one element");
    }
}

internal sealed class CreateSubscriptionCommandHandler(
    IReadSubscriptionRepository readSubscriptionRepository,
    IWriteSubscriptionRepository writeSubscriptionRepository) : ICommandHandler<CreateSubscriptionCommand>
{
    public async Task HandleAsync(CreateSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        var doesEmailExist = await readSubscriptionRepository.DoesTitleExistAsync(command.Title, cancellationToken);
        
        if (doesEmailExist)
        {
            return;
        }

        var subscription = Subscription.Create(command.Id, command.Title, command.PricePerMonth,
            command.PricePerYear, command.Features);
        await writeSubscriptionRepository.AddAsync(subscription, cancellationToken);
    }
}