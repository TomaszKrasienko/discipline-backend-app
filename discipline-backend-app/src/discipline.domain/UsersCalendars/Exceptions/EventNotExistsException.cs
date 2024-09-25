using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class EventNotExistsException(Guid eventId) 
    : DisciplineException($"Event with ID: {eventId} not found");