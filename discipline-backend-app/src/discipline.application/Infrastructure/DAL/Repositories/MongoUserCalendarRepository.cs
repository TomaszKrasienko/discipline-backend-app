using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.Repositories;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoUserCalendarRepository(
    ) : IUserCalendarRepository
{
    public Task AddAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserCalendar userCalendar, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserCalendar> GetByDateAsync(DateOnly day, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}