using discipline.domain.UsersCalendars.Exceptions;
using discipline.domain.UsersCalendars.Repositories;
using discipline.domain.UsersCalendars.Services.Abstractions;

namespace discipline.domain.UsersCalendars.Services;

internal sealed class ChangeEventUserCalendarService(
    IUserCalendarRepository userCalendarRepository) : IChangeEventUserCalendarService
{
    public async Task Invoke(Guid userId, Guid eventId, DateOnly newDate, CancellationToken cancellationToken)
    {
        var oldUserCalendar = await userCalendarRepository.GetByEventIdAsync(userId, eventId, cancellationToken);
        if (oldUserCalendar is null)
        {
            throw new UserCalendarForEventNotFoundException(userId, eventId);
        }
    }
}