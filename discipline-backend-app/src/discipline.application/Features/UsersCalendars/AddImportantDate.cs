using discipline.application.Behaviours;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.UsersCalendars;

internal static class AddImportantDate
{
    internal static WebApplication MapAddImportantDate(this WebApplication app)
    {
        app.MapPost("user-calendar/add-important-date", async (AddImportantDateCommand command,
                    ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var eventId = Guid.NewGuid();
                await commandDispatcher.HandleAsync(command, cancellationToken);
                return Results.CreatedAtRoute(nameof(GetEventById), new {eventId = eventId}, null);
            })
        .Produces(StatusCodes.Status201Created, typeof(void))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
        .WithName(nameof(AddImportantDate))
        .WithOpenApi(operation => new (operation)
        {
            Description = "Adds event and adds or updates user calendar"
        });;
        return app;
    }
}

public sealed record AddImportantDateCommand(DateOnly Day, Guid Id, string Title) : ICommand;

public sealed class AddImportantDateCommandValidator : AbstractValidator<AddImportantDateCommand>
{
    public AddImportantDateCommandValidator()
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
    }
}

internal sealed class AddImportantDateCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<AddImportantDateCommand>
{
    public async Task HandleAsync(AddImportantDateCommand command, CancellationToken cancellationToken = default)
    {
        var userCalendar = await userCalendarRepository.GetByDateAsync(command.Day, cancellationToken);
        if (userCalendar is null)
        {
            userCalendar = UserCalendar.Create(command.Day);
            userCalendar.AddEvent(command.Id, command.Title);
            await userCalendarRepository.AddAsync(userCalendar, cancellationToken);
            return;
        }
        userCalendar.AddEvent(command.Id, command.Title);
        await userCalendarRepository.UpdateAsync(userCalendar, cancellationToken);
    }
}