namespace discipline.domain.UsersCalendars.Services.Abstractions;

public interface IChangeEventUserCalendarService
{
    Task Invoke(Ulid userId, Ulid eventId, DateOnly newDate, CancellationToken cancellationToken);
}