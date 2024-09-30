namespace discipline.domain.UsersCalendars.Services.Abstractions;

public interface IChangeEventUserCalendarService
{
    Task Invoke(Guid eventId, DateOnly newDate);
}