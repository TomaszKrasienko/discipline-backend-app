using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.domain.UsersCalendars.Repositories;
using FluentValidation;

namespace discipline.application.Features.UsersCalendars;

internal sealed class EditCalendarEvent
{
    
}

public sealed record EditCalendarEventCommand(Guid UserId, Guid Id, string Title, TimeOnly TimeFrom,
    TimeOnly? TimeTo, string Action) : ICommand;

public sealed class EditCalendarEventCommandValidator : AbstractValidator<EditCalendarEventCommand>
{
    public EditCalendarEventCommandValidator()
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

        await userCalendarRepository.UpdateAsync(userCalendar, cancellationToken);
    }
}