using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Behaviours.IdentityContext;
using discipline.application.Exceptions;
using discipline.application.Features.UsersCalendars.Configuration;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.application.Features.UsersCalendars;

internal static class EditCalendarEvent
{
    internal static WebApplication MapEditCalendarEvent(this WebApplication app)
    {
        app.MapPut($"{Extensions.UserCalendarTag}/edit-calendar-event/{{eventId}}", async (EditCalendarEventCommand command,
            Ulid eventId, IIdentityContext identityContext, ICqrsDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command with
                {
                    UserId = identityContext.UserId,
                    Id = new EventId(eventId)
                }, cancellationToken);
            })
        .Produces(StatusCodes.Status200OK, typeof(void))
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status401Unauthorized, typeof(void))
        .Produces(StatusCodes.Status403Forbidden, typeof(void))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
        .WithName(nameof(EditCalendarEvent))
        .WithTags(Extensions.UserCalendarTag)
        .WithOpenApi(operation => new (operation)
        {
            Description = "Edits calendar event"
        })
        .RequireAuthorization()
        .RequireAuthorization(UserStatePolicy.Name);
        return app;
    }
}

public sealed record EditCalendarEventCommand(UserId UserId, EventId Id, string Title, TimeOnly TimeFrom,
    TimeOnly? TimeTo, string Action) : ICommand;

public sealed class EditCalendarEventCommandValidator : AbstractValidator<EditCalendarEventCommand>
{
    public EditCalendarEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(id => id != new EventId(Ulid.Empty))
            .WithMessage("Important date \"ID\" can not be empty");

        RuleFor(x => x.UserId)
            .Must(userId => userId != new UserId(Ulid.Empty))
            .WithMessage("Important date \"UserId\" can not be empty");
        
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Important date \"Title\" can not be null or empty");

        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Important date \"Title\" has invalid length");

        RuleFor(x => x.TimeFrom)
            .NotEqual((TimeOnly)default)
            .WithMessage("Time from can not be empty");
    }
}

internal sealed class EditCalendarEventCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<EditCalendarEventCommand>
{
    public async Task HandleAsync(EditCalendarEventCommand command, CancellationToken cancellationToken = default)
    {
        var userCalendar = await userCalendarRepository
            .GetByEventIdAsync(command.UserId, command.Id, cancellationToken);
        if (userCalendar is null)
        {
            throw new UserCalendarNotFoundException(command.UserId, command.Id);
        }

        userCalendar.EditEvent(command.Id, command.Title, command.TimeFrom, command.TimeTo, command.Action);
        await userCalendarRepository.UpdateAsync(userCalendar, cancellationToken);
    }
}