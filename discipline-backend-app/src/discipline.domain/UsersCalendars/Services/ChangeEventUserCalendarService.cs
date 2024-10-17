using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Exceptions;
using discipline.domain.UsersCalendars.Repositories;
using discipline.domain.UsersCalendars.Services.Abstractions;

namespace discipline.domain.UsersCalendars.Services;

internal sealed class ChangeEventUserCalendarService(
    IUserCalendarRepository userCalendarRepository) : IChangeEventUserCalendarService
{
    public async Task Invoke(UserId userId, EventId eventId, DateOnly newDate, CancellationToken cancellationToken)
    {
        var oldUserCalendar = await userCalendarRepository.GetByEventIdAsync(userId, eventId, cancellationToken);
        if (oldUserCalendar is null)
        {
            throw new UserCalendarForEventNotFoundException(userId, eventId);
        }

        var @event = oldUserCalendar.Events.First(x => x.Id == eventId);
        oldUserCalendar.RemoveEvent(eventId);

        UserCalendar newUserCalendar = await userCalendarRepository
            .GetForUserByDateAsync(userId, newDate, cancellationToken);
        var isNewUserCalendarExists = true;
        if (newUserCalendar is null)
        {
            //TODO: Change
            newUserCalendar = UserCalendar.Create(UserCalendarId.New(), newDate, userId);
            isNewUserCalendarExists = false;
        }
        newUserCalendar.AddEvent(@event);

        if (isNewUserCalendarExists)
        {
            await userCalendarRepository.UpdateAsync(newUserCalendar, cancellationToken);
        }
        else
        {
            await userCalendarRepository.AddAsync(newUserCalendar, cancellationToken);   
        }
        await userCalendarRepository.UpdateAsync(oldUserCalendar, cancellationToken);
    }
}