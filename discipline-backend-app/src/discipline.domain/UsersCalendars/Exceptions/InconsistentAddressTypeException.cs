using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class InconsistentAddressTypeException()
    : DisciplineException("Address type must be online on onside");