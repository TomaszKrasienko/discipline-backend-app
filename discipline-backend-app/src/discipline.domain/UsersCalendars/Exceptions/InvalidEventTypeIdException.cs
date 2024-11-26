using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class InvalidEventTypeIdException(EventId id)
    : DisciplineException($"Calendar event with id: {id.ToString()} has invalid type");