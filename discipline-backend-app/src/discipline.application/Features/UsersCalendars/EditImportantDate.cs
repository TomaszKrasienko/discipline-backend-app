using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.UsersCalendars.Configuration;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class EditImportantDate
{
    internal static WebApplication MapEditImportantDate(this WebApplication app)
    {
        app.MapPut($"{Extensions.UserCalendarTag}/edit-important-date/{{eventId:guid}}", async (EditImportantDateCommand command,
                Guid eventId, IIdentityContext identityContext, ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command with
                {
                    UserId = identityContext.UserId,
                    Id = eventId
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

public sealed record EditImportantDateCommand(Guid UserId, Guid Id, string Title) : ICommand;

public sealed class EditImportantDateCommandValidator : AbstractValidator<EditImportantDateCommand>
{
    public EditImportantDateCommandValidator()
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