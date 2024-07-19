using discipline.application.Behaviours;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class AddCalendarEvent
{
    internal static WebApplication MapAddCalendarEvent(this WebApplication app)
    {
        app.MapPost("user-calendar/add-calendar-event", async (AddCalendarEventCommand command,
            HttpContext httpContext, ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
        {
            var eventId = Guid.NewGuid();
            await commandDispatcher.HandleAsync(command with { Id = eventId }, cancellationToken);
            httpContext.AddResourceIdHeader(eventId);
            return Results.CreatedAtRoute(nameof(GetEventById), new {eventId = eventId}, null);
        })
        .Produces(StatusCodes.Status201Created, typeof(void))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
        .WithName(nameof(AddCalendarEvent))
        .WithOpenApi(operation => new (operation)
        {
            Description = "Adds calendar event to existing user calendar for day or creates user calendar for day"
        });
        return app;
    }
}

public sealed record AddCalendarEventCommand(DateOnly Day, Guid Id, string Title, TimeOnly TimeFrom,
    TimeOnly? TimeTo, string Action) : ICommand;


public sealed class AddCalendarEventCommandValidator : AbstractValidator<AddCalendarEventCommand>
{
    public AddCalendarEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Important date \"ID\" can not be empty");

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

internal sealed class AddCalendarEventCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<AddCalendarEventCommand>
{
    public async Task HandleAsync(AddCalendarEventCommand command, CancellationToken cancellationToken = default)
    {
        var userCalendar = await userCalendarRepository.GetByDateAsync(command.Day, cancellationToken);
        if (userCalendar is null)
        {
            userCalendar = UserCalendar.Create(command.Day);
            userCalendar.AddEvent(command.Id, command.Title, command.TimeFrom, command.TimeTo,
                command.Action);
            await userCalendarRepository.AddAsync(userCalendar, cancellationToken);
            return;
        }
        userCalendar.AddEvent(command.Id, command.Title, command.TimeFrom, command.TimeTo,
            command.Action);
        await userCalendarRepository.UpdateAsync(userCalendar, cancellationToken);
    }
}
