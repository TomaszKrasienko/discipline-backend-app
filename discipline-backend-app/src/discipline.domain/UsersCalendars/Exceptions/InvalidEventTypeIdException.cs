using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class InvalidEventTypeIdException(Guid id)
    : DisciplineException($"Calendar event with id: {id} has invalid type");