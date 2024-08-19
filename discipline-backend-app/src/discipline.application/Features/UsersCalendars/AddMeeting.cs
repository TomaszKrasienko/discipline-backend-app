using discipline.application.Behaviours;
using discipline.application.Features.UsersCalendars.Configuration;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

public static class AddMeeting
{
    internal static WebApplication MapAddMeeting(this WebApplication app)
    {
        app.MapPost($"{Extensions.UserCalendarTag}/add-meeting", async (AddMeetingCommand command,
                HttpContext httpContext, IIdentityContext identityContext, ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var eventId = Guid.NewGuid();
                await commandDispatcher.HandleAsync(command with
                {
                    Id = eventId,
                    UserId = identityContext.UserId
                }, cancellationToken);
                httpContext.AddResourceIdHeader(eventId);
                return Results.CreatedAtRoute(nameof(GetEventById), new {eventId = eventId}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))            
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(AddMeeting))
            .WithTags(Extensions.UserCalendarTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds meeting to existing user calendar for day or creates user calendar for day"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);;
        return app;
    }
}

public sealed record AddMeetingCommand(DateOnly Day, Guid UserId, Guid Id, string Title, TimeOnly TimeFrom,
    TimeOnly? TimeTo, string Platform, string Uri, string Place) : ICommand;

public sealed class AddMeetingCommandValidator : AbstractValidator<AddMeetingCommand>
{
    public AddMeetingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Important date \"ID\" can not be empty");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
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
            userCalendar = UserCalendar.Create(command.Day, command.UserId);
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