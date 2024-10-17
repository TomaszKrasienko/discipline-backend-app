using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class UserCalendarForEventNotFoundException(Ulid userId, Ulid eventId)
    : DisciplineException($"User calendar for event with ID: {eventId} and user with ID: {userId} not found");