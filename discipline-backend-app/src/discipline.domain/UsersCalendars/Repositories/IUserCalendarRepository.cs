using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;

namespace discipline.domain.UsersCalendars.Repositories;

public interface IUserCalendarRepository
{
    Task AddAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default);
    Task<UserCalendar> GetForUserByDateAsync(UserId userId, DateOnly day, CancellationToken cancellationToken = default);
    Task<UserCalendar> GetByEventIdAsync(UserId userId, EventId eventId, CancellationToken cancellationToken = default);
}