using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class UserCalendarForEventNotFoundException(UserId userId, EventId eventId)
    : DisciplineException($"User calendar for event with ID: {eventId} and user with ID: {userId} not found");