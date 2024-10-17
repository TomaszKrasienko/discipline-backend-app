using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class EventNotFoundException(Ulid eventId) 
    : DisciplineException($"Event with ID: {eventId} not found");