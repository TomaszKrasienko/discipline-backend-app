using discipline.domain.UsersCalendars.Entities;

namespace discipline.domain.UsersCalendars.Repositories;

internal interface IUserCalendarRepository
{
    Task AddAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default);
    Task<UserCalendar> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default);
}