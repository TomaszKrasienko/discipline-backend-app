using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class InvalidEventTypeIdException(Ulid id)
    : DisciplineException($"Calendar event with id: {id} has invalid type");