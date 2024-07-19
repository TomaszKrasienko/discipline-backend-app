using discipline.application.Behaviours;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

public static class AddMeeting
{
    internal static WebApplication MapAddMeeting(this WebApplication app)
    {
        app.MapPost("user-calendar/add-meeting", async (AddMeetingCommand command,
                HttpContext httpContext, ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var eventId = Guid.NewGuid();
                await commandDispatcher.HandleAsync(command with {Id = eventId}, cancellationToken);
                httpContext.AddResourceIdHeader(eventId);
                return Results.CreatedAtRoute(nameof(GetEventById), new {eventId = eventId}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(AddMeeting))
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds meeting to existing user calendar for day or creates user calendar for day"
            });
        return app;
    }
}

public sealed record AddMeetingCommand(DateOnly Day, Guid Id, string Title, TimeOnly TimeFrom,
    TimeOnly? TimeTo, string Platform, string Uri, string Place) : ICommand;

public sealed class AddMeetingCommandValidator : AbstractValidator<AddMeetingCommand>
{
    public AddMeetingCommandValidator()
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
    
internal sealed class AddMeetingCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<AddMeetingCommand>
{
    public async Task HandleAsync(AddMeetingCommand command, CancellationToken cancellationToken = default)
    {
        var userCalendar = await userCalendarRepository.GetByDateAsync(command.Day, cancellationToken);
        if (userCalendar is null)
        {
            userCalendar = UserCalendar.Create(command.Day);
            userCalendar.AddEvent(command.Id, command.Title, command.TimeFrom, command.TimeTo,
                command.Platform, command.Uri, command.Place);
            await userCalendarRepository.AddAsync(userCalendar, cancellationToken);
            return;
        }
        userCalendar.AddEvent(command.Id, command.Title, command.TimeFrom, command.TimeTo,
            command.Platform, command.Uri, command.Place);
        await userCalendarRepository.UpdateAsync(userCalendar, cancellationToken);
    }
}