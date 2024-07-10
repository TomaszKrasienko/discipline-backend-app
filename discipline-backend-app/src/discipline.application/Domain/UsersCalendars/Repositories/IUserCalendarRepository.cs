using discipline.application.Domain.UsersCalendars.Entities;

namespace discipline.application.Domain.UsersCalendars.Repositories;

internal interface IUserCalendarRepository
{
    Task AddAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default);
    Task<UserCalendar> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default);
}