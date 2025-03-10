namespace discipline.centre.calendar.domain.Repositories;

public interface IReadWriteUserCalendarRepository : IReadUserCalendarRepository
{
    Task AddAsync(UserCalendarDay userCalendarDay, CancellationToken cancellationToken);
    Task UpdateAsync(UserCalendarDay userCalendarDay, CancellationToken cancellationToken);
}