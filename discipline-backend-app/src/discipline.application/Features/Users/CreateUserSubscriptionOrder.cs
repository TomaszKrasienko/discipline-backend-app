using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.Users.Configuration;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Repositories;
using discipline.domain.Users.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.Users;

public static class CreateUserSubscriptionOrder
{
    internal static WebApplication MapCreateUserSubscriptionOrder(this WebApplication app)
    {
        app.MapPost($"{Extensions.UsersTag}/crate-subscription-order", async (CreateUserSubscriptionOrderCommand command,
            IIdentityContext identityContext, ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var subscriptionOrderId = Guid.NewGuid();
                await commandDispatcher.HandleAsync(command with { Id = subscriptionOrderId, UserId = identityContext.UserId },
                    cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(CreateUserSubscriptionOrder))
            .WithTags(Extensions.UsersTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds subscription order for user"
            })
            .RequireAuthorization();
        return app;
    }
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
    }
}

internal sealed class CreateUserSubscriptionOrderCommandHandler(
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository,
    ISubscriptionOrderService subscriptionOrderService,
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
        
        subscriptionOrderService.AddOrderSubscriptionToUser(user, command.Id, subscription,
            command.SubscriptionOrderFrequency, clock.DateNow(), command.CardNumber, command.CardCvvNumber);
        await userRepository.UpdateAsync(user, cancellationToken);
    }
}