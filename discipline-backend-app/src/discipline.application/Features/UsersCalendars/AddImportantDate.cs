using discipline.application.Behaviours;
using discipline.application.Features.UsersCalendars.Configuration;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class AddImportantDate
{
    internal static WebApplication MapAddImportantDate(this WebApplication app)
    {
        app.MapPost($"{Extensions.UserCalendarTag}/add-important-date", async (AddImportantDateCommand command,
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
            .WithName(nameof(AddImportantDate))
            .WithTags(Extensions.UserCalendarTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds important date to existing user calendar for day or creates user calendar for day"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);;
        return app;
    }
}

public sealed record AddImportantDateCommand(DateOnly Day, Guid UserId, Guid Id, string Title) : ICommand;

public sealed class AddImportantDateCommandValidator : AbstractValidator<AddImportantDateCommand>
{
    public AddImportantDateCommandValidator()
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
    }
}

internal sealed class AddImportantDateCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<AddImportantDateCommand>
{
    public async Task HandleAsync(AddImportantDateCommand command, CancellationToken cancellationToken = default)
    {
        var userCalendar = await userCalendarRepository.GetForUserByDateAsync(command.UserId, command.Day, cancellationToken);
        if (userCalendar is null)
        {
            userCalendar = UserCalendar.Create(command.Day, command.UserId);
            userCalendar.AddEvent(command.Id, command.Title);
            await userCalendarRepository.AddAsync(userCalendar, cancellationToken);
            return;
        }
        userCalendar.AddEvent(command.Id, command.Title);
        await userCalendarRepository.UpdateAsync(userCalendar, cancellationToken);
    }
}