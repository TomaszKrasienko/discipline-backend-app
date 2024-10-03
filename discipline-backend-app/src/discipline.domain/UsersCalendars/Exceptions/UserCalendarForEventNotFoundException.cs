using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class UserCalendarForEventNotFoundException(Guid userId, Guid eventId)
    : DisciplineException($"User calendar for event with ID: {eventId} and user with ID: {userId} not found");