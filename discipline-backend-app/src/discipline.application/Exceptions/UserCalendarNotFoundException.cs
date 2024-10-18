using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class UserCalendarNotFoundException(Guid userId, Guid id)
    :  DisciplineException($"User calendar for userId: {userId} and with id: {id} not exists");