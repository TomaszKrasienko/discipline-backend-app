namespace discipline.domain.UsersCalendars.Services.Abstractions;

public interface IChangeEventUserCalendarService
{
    Task Invoke(Guid userId, Guid eventId, DateOnly newDate, CancellationToken cancellationToken);
}