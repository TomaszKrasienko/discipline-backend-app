using discipline.domain.UsersCalendars.Entities;

namespace discipline.domain.UsersCalendars.Repositories;

public interface IUserCalendarRepository
{
    Task AddAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default);
    Task<UserCalendar> GetForUserByDateAsync(Ulid userId, DateOnly day, CancellationToken cancellationToken = default);
    Task<UserCalendar> GetByEventIdAsync(Ulid userId, Ulid eventId, CancellationToken cancellationToken = default);
}