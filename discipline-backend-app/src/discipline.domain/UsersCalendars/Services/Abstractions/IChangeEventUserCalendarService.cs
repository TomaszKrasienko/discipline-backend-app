using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.UsersCalendars.Services.Abstractions;

public interface IChangeEventUserCalendarService
{
    Task Invoke(UserId userId, EventId eventId, DateOnly newDate, CancellationToken cancellationToken);
}