using discipline.centre.calendar.domain;
using discipline.centre.calendar.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.application.UserCalendar.TimeEvents.Commands.CreateTimeEvent;

public sealed record CreateTimeEventCommand(UserId UserId, 
    DateOnly Day,
    CalendarEventId EventId, 
    string Title, 
    string? Description,
    TimeOnly TimeFrom,
    TimeOnly? TimeTo) : ICommand;
    
internal sealed class CreateTimeEventCommandHandler(
    IReadWriteUserCalendarRepository readWriteUserCalendarRepository) : ICommandHandler<CreateTimeEventCommand>
{
    public async Task HandleAsync(CreateTimeEventCommand command, CancellationToken cancellationToken)
    {
        var userCalendarDay = await readWriteUserCalendarRepository.GetByDayAsync(command.UserId, command.Day, cancellationToken);

        if (userCalendarDay is null)
        {
            userCalendarDay = UserCalendarDay.CreateWithTimeEvent(UserCalendarId.New(), command.UserId, command.Day,
                command.EventId, command.Title, command.Description, command.TimeFrom, command.TimeTo);
            
            await readWriteUserCalendarRepository.AddAsync(userCalendarDay, cancellationToken);
        }
        else
        {
            userCalendarDay.AddTimeEvent(command.EventId, command.Title, command.Description, command.TimeFrom, command.TimeTo);
            
            await readWriteUserCalendarRepository.UpdateAsync(userCalendarDay, cancellationToken);
        }
    }
}