using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.UsersCalendars.Configuration;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class EditImportantDate
{
    internal static WebApplication MapEditImportantDate(this WebApplication app)
    {
        app.MapPut($"{Extensions.UserCalendarTag}/edit-important-date/{{eventId}}", async (EditImportantDateCommand command,
                Ulid eventId, IIdentityContext identityContext, ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command with
                {
                    UserId = identityContext.UserId,
                    Id = new EventId(eventId)
                }, cancellationToken);
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(EditImportantDate))
            .WithTags(Extensions.UserCalendarTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Edits important date"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);
        return app;
    }
}

public sealed record EditImportantDateCommand(UserId UserId, EventId Id, string Title) : ICommand;

public sealed class EditImportantDateCommandValidator : AbstractValidator<EditImportantDateCommand>
{
    public EditImportantDateCommandValidator()
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
    }
}

internal sealed class EditImportantDateCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<EditImportantDateCommand>
{
    public async Task HandleAsync(EditImportantDateCommand command, CancellationToken cancellationToken = default)
    {
        var userCalendar = await userCalendarRepository
            .GetByEventIdAsync(command.UserId, command.Id, cancellationToken);
        if (userCalendar is null)
        {
            throw new UserCalendarNotFoundException(command.UserId, command.Id);
        }

        userCalendar.EditEvent(command.Id, command.Title);
        await userCalendarRepository.UpdateAsync(userCalendar, cancellationToken);
    }
}