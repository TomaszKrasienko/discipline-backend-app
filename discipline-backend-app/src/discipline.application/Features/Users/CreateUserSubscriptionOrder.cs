using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Behaviours.IdentityContext;
using discipline.application.Behaviours.Time;
using discipline.application.Exceptions;
using discipline.application.Features.Users.Configuration;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Repositories;
using discipline.domain.Users.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.application.Features.Users;

public static class CreateUserSubscriptionOrder
{
    internal static WebApplication MapCreateUserSubscriptionOrder(this WebApplication app)
    {
        app.MapPost($"{Extensions.UsersTag}/create-subscription-order", async (CreateUserSubscriptionOrderCommand command,
            IIdentityContext identityContext, ICqrsDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var subscriptionOrderId = SubscriptionOrderId.New();
                await commandDispatcher.HandleAsync(command with { Id = subscriptionOrderId, UserId = identityContext.UserId },
                    cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
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

public sealed record CreateUserSubscriptionOrderCommand(UserId UserId, SubscriptionOrderId Id, 
    SubscriptionId SubscriptionId, SubscriptionOrderFrequency? SubscriptionOrderFrequency,
    string CardNumber, string CardCvvNumber) : ICommand;

public sealed class CreateUserSubscriptionOrderCommandValidator : AbstractValidator<CreateUserSubscriptionOrderCommand>
{
    public CreateUserSubscriptionOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(userId => userId != new UserId(Ulid.Empty))
            .WithMessage("\"User Id\" can not be empty");

        RuleFor(x => x.Id)
            .Must(id => id != new SubscriptionOrderId(Ulid.Empty))
            .WithMessage("\"Id\" can not be empty");
        
        RuleFor(x => x.SubscriptionId)
            .Must(subscriptionId => subscriptionId != new SubscriptionId(Ulid.Empty))
            .WithMessage("\"SubscriptionId\" can not be empty");
    }
}

internal sealed class CreateUserSubscriptionOrderCommandHandler(
    IReadUserRepository readUserRepository,
    IWriteUserRepository writeUserRepository,
    ISubscriptionRepository subscriptionRepository,
    ISubscriptionOrderService subscriptionOrderService,
    IClock clock) : ICommandHandler<CreateUserSubscriptionOrderCommand>
{
    public async Task HandleAsync(CreateUserSubscriptionOrderCommand command, CancellationToken cancellationToken = default)
    {
        var user = await readUserRepository.GetByIdAsync(command.UserId, cancellationToken);
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
            command.SubscriptionOrderFrequency, clock.DateTimeNow(), command.CardNumber, command.CardCvvNumber);
        await writeUserRepository.UpdateAsync(user, cancellationToken);
    }
}