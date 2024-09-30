using discipline.domain.UsersCalendars.Repositories;
using discipline.domain.UsersCalendars.Services.Abstractions;

namespace discipline.domain.UsersCalendars.Services;

internal sealed class ChangeEventUserCalendarService(
    IUserCalendarRepository userCalendarRepository) : IChangeEventUserCalendarService
{
    public Task Invoke(Guid eventId, DateOnly newDate)
    {
        throw new NotImplementedException();
    }
}