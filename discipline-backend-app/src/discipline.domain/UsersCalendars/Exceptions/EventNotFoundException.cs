using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class EventNotFoundException(EventId eventId) 
    : DisciplineException($"Event with ID: {eventId} not found");