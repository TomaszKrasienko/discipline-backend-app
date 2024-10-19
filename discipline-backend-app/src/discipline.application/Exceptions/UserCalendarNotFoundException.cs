using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.application.Exceptions;

public sealed class UserCalendarNotFoundException(UserId userId, EventId id)
    :  DisciplineException($"User calendar for userId: {userId.ToString()} and with id: {id.ToString()} not exists");