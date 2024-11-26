using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Behaviours.IdentityContext;
using discipline.application.Features.UsersCalendars.Configuration;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.application.Features.UsersCalendars;

public static class AddMeeting
{
    internal static WebApplication MapAddMeeting(this WebApplication app)
    {
        app.MapPost($"{Extensions.UserCalendarTag}/add-meeting", async (AddMeetingCommand command,
                HttpContextAccessor httpContext, IIdentityContext identityContext, ICqrsDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var eventId = EventId.New();
                await commandDispatcher.HandleAsync(command with
                {
                    Id = eventId,
                    UserId = identityContext.UserId
                }, cancellationToken);
                httpContext.AddResourceIdHeader(eventId.ToString());
                return Results.CreatedAtRoute(nameof(GetEventById), new {eventId = eventId}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))            
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName(nameof(AddMeeting))
            .WithTags(Extensions.UserCalendarTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds meeting to existing user calendar for day or creates user calendar for day"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        return app;
    }
}

public sealed record AddMeetingCommand(DateOnly Day, UserId UserId, EventId Id, string Title, TimeOnly TimeFrom,
    TimeOnly? TimeTo, string Platform, string Uri, string Place) : ICommand;

public sealed class AddMeetingCommandValidator : AbstractValidator<AddMeetingCommand>
{
    public AddMeetingCommandValidator()
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
    
internal sealed class AddMeetingCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<AddMeetingCommand>
{
    public async Task HandleAsync(AddMeetingCommand command, CancellationToken cancellationToken = default)
    {
        var userCalendar = await userCalendarRepository.GetForUserByDateAsync(command.UserId, command.Day, cancellationToken);
        if (userCalendar is null)
        {
            userCalendar = UserCalendar.Create(UserCalendarId.New(), command.Day, command.UserId);
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