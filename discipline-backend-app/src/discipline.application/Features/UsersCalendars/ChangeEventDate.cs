using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Features.UsersCalendars.Configuration;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class ChangeEventDate
{
    internal static WebApplication MapChangeEventDate(this WebApplication app)
    {
        app.MapPatch($"{Extensions.UserCalendarTag}/event/{{eventId}}/change-event-date", async (
                Ulid eventId, ChangeEventDateCommand command, ICqrsDispatcher commandDispatcher,
                IIdentityContext identityContext, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command with
                {
                    UserId = identityContext.UserId,
                    EventId = new EventId(eventId)
                }, cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(ChangeEventDate))
            .WithTags(Extensions.UserCalendarTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Changes event date"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);
        return app;
    }
}

public sealed record ChangeEventDateCommand(UserId UserId, EventId EventId, DateOnly NewDate) : ICommand;

public sealed class ChangeEventDateCommandValidator : AbstractValidator<ChangeEventDateCommand>
{
    public ChangeEventDateCommandValidator()
    { 
        RuleFor(x => x.UserId)
            .Must(userId => userId != new UserId(Ulid.Empty))
            .WithMessage("\"UserId\" can not be empty");
        
        RuleFor(x => x.EventId)
            .Must(id => id != new EventId(Ulid.Empty))
            .WithMessage("\"EventId\" can not be empty");

        RuleFor(x => x.NewDate)
            .NotEqual(DateOnly.MinValue)
            .WithMessage("\"NewDate\" can not be min date");
    }
}

internal sealed class ChangeEventDateCommandHandler(
    IChangeEventUserCalendarService service) : ICommandHandler<ChangeEventDateCommand>
{
    public async Task HandleAsync(ChangeEventDateCommand command, CancellationToken cancellationToken = default)
        => await service.Invoke(command.UserId, command.EventId, command.NewDate, cancellationToken);
    
}