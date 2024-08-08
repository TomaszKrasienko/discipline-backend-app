using discipline.application.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.application.Domain.UsersCalendars.Exceptions;

public sealed class InconsistentAddressTypeException()
    : DisciplineException("Address type must be online on onside");